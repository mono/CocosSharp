# CocosSharp v1.2.1.0 release notes 

## Breaking changes 
 ---
None

## Key new feature: TexelToContentSizeRatio/s
 ---
When loading assets such as sprites the <code>ContentSize</code> defaults to the texel dimensions of the original image. However, this becomes problematic when developers wish to include high/low-def variants of their images when targeting different screen resolutions but who wish to maintain a consistent content size. To accommodate for this we have introduced the static properties <code>TexelToContentSizeRatios<code> of type <code>CCSize</code> and the <code>TexelToContentSizeRatio</code> of type <code>float</code> to allow users to adjust the scale of conversion between texels to content size. These properties are available hierarchically throughout a number of classes - namely,

* CCApplication
    * CCSprite
    * CCTMXLayer
    * CCLabelTTf
    * CCLabelBMFont

To use these properties, within the overridden <code>CCApplicationDelegate</code> one could do the following

<pre>
<code>
    public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
    {

        // Other initialisation
        // ...
        CCSize windowSize = mainWindow.WindowSizeInPixels;

        float desiredWidth = 1024.0f;
        float desiredHeight = 768.0f;

        // This will set the world bounds to be (0,0, w, h) - ContentSize is relative to these dimensions
        mainWindow.SetDesignResolutionSize(desiredWidth, desiredHeight, CCSceneResolutionPolicy.ShowAll);

        if (desiredWidth < windowSize.Width)
        {
            application.ContentSearchPaths.Add("images/hd");
            CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
        }
        else
        {
            application.ContentSearchPaths.Add("images/ld");
            CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
        }

        // ...
</code>
</pre>

Also, the Xamarin Studio project templates have been updated to incorporate these changes so you'll be able to quickly setup a project to handle multiple screen resolutions.


## Fixes and enhancements 
 ---
* [60](https://github.com/mono/CocosSharp/issues/60) XS Project Templates: Showcase new TexelToContentSizeRatio feature
* [59](https://github.com/mono/CocosSharp/issues/59) Visibility of added children are not being recursively set from parent.
* [58](https://github.com/mono/CocosSharp/issues/58) Issues with NuGets in Templates (Mobile Apps -> Empty Project)
* [51](https://github.com/mono/CocosSharp/issues/51) CCLabel: offset issue
* [50](https://github.com/mono/CocosSharp/issues/50) CCSprite: Add TexelToContentSizeRatio
* [49](https://github.com/mono/CocosSharp/issues/49) CCSimpleAudioEngine: Small music file plays twice
