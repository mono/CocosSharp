#!/usr/bin/env python
#
# Author: Rami Tabbara
#
# CocosSharp
# Use GitHub API to find closed issues between releases and generate markdown release notes
#
import urllib, json, dateutil.parser, os, sys, getopt

repo_issues_root_url = "https://api.github.com/repos/mono/CocosSharp/"
repo_root_url = repo_issues_root_url + "git/"

def usage():
	print "\nCocosSharp release notes generator\n"
 	print 'Usage: '+sys.argv[0]+' -s [start version tag] -e [end version tag] -o [output path]'
	print '\nArguments:\n'
	print '\t-s or --startversion\t\tThe starting version tag to begin release notes generation'
	print '\t-e or --endversion\t\tThe ending version tag to end release notes generation'
	print '\t-o or --outputpath\t\tThe output path. Defaults to current directory'
	print '\t-h or --help\t\t\tPrint this message and exit'
	print 'Sample:\n'
	print '\t' + sys.argv[0] + ' -s v1.0.1.0 -e v1.1.0.0 -o mypath/'

def main():
	
	start_version = ""
	end_version = ""
	outputpath = "./"
	
	try:
		opts, args = getopt.getopt(sys.argv[1:], "hs:e:o:", ["help", "startversion=", "endversion=", "outputpath="])
	except getopt.GetoptError:
		usage()
		sys.exit(2)
	for opt, arg in opts:
		if opt in ("-h", "--help"):
			usage()
			sys.exit()
		elif opt in ("-s", "--startversion"):
			start_version = arg
		elif opt in ("-e", "--endversion"):
			end_version = arg
		elif opt in ("-o", "--outputpath"):
			outputpath = arg

	if start_version == "" or end_version == "" or outputpath == "":
		usage()
		sys.exit(2)
	
	print("Started generating release notes")
	start_date = find_tag_commit_date_string(start_version)
	end_date = find_tag_commit_date_string(end_version)
	closed_issues = find_closed_issues(start_date, end_date)
	
	generate_release_notes(end_version, closed_issues, './')

def fetch_github_json_data(request_str):
	response = urllib.urlopen(request_str)
	return json.loads(response.read())

# Find dates when corresponding commits were created
def find_tag_commit_date_string(tag_str):
	print("Finding corresponding commit date for tag " + tag_str)
	
	# Find tag's hash
	tag_ref_root_url = repo_root_url + "refs/tags/" + tag_str
	tag_ref_data = fetch_github_json_data(tag_ref_root_url)
	tag_sha = tag_ref_data['object']['sha']
	
	# Find corresponding commit's hash
	tag_url = repo_root_url + "tags/" + tag_sha
	tag_data = fetch_github_json_data(tag_url)
	commit_sha = tag_data['object']['sha']
	
	# Find commit date
	commit_url = repo_root_url + "commits/" + commit_sha
	commit_data = fetch_github_json_data(commit_url)
	commit_date = commit_data['committer']['date']
	
	print(tag_str + ' :' + commit_date)
	
	return commit_date

# Find isses that were closed after startingVersion date but not after endVersion date
def date_str_to_date(date_str):
	return dateutil.parser.parse(date_str)
	
def find_closed_issues(start_date_str, end_date_str):
	print('Fetching closed issues between ' + start_date_str + ' and ' + end_date_str)
	end_date = date_str_to_date(end_date_str)
	issues_url = repo_issues_root_url + "issues?state=closed&sort=updated&since=" + start_date_str
	issues_data = fetch_github_json_data(issues_url)
	filtered_issues = [issue for issue in issues_data if date_str_to_date(issue['closed_at']) < end_date]
	print(str(len(filtered_issues)) + ' closed issues found.')
	
	return filtered_issues
	
# Generate markdown from closed issues
def generate_release_notes(version_tag, list_of_issues, output_path):
	filename = 'ReleaseNotes_' + version_tag + '.md'
	filepath = os.path.join(output_path, filename)
	print("Started writing release notes to path: " + filepath)
	with open(filepath, 'w') as file:
		file.write('# CocosSharp ' + version_tag + ' release notes \n')
		file.write('## Fixes and enhancements \n ---\n')
		for issue_dict in list_of_issues:
			title = issue_dict['title']
			issue_num = str(issue_dict['number'])
			html_url =  issue_dict['html_url']
			file.write('* ' +  '[' + issue_num + '](' + html_url +') ' + title + '\n')
	print("Finished writing release notes")
		
if __name__ == '__main__': 
	main()