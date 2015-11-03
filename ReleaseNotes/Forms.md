
# CocosSharp 1.7.0.0: Embeddable game view and Xamarin.Forms support

One of the most highly sought after features requested by users was the ability to integrate Xamarin.Forms within a CocosSharp game. Specifically, users were after a way to incorporate Xamarin.Forms controls alongside an embeddable game view that does not necessarily occupy the entire screen.

Unfortunately, due to a variety of limitations imposed by our dependencies, prior to CocosSharp 1.7.0.0, such functionality was previously not possible. Hence, to realise our goal, we have had to start from the ground-up and substantially redesign how a CocosSharp game is initialised.

## The end of CCApplication &mdash; introducing the new CCGameView

Users of CocosSharp will be familiar with <code>CCApplication</code>'s role in kick-starting their game. For example, on iOS, we would do the following:

<pre>
<code>
    class Program : NSApplicationDelegate 
    {
        public override void DidFinishLaunching (MonoMac.Foundation.NSNotification notification)
        {

            CCApplication application = new CCApplication(false, new CCSize(1024f, 768f));
            
            // GameAppDelegate is a subclass of CCApplicationDelegate, a container class for loading the game scene
            application.ApplicationDelegate = new GameAppDelegate();

            application.StartGame();
        }

        ....
</code>
</pre>

The problem implicit in this setup was that <code>CCApplication</code> took full control over your application, insisting that your game view was not only full-screen but that was it was the sole, root view displayed.

In CocosSharp 1.7.0.0, we have replaced <code>CCApplication</code> with the new <code>CCGameView</code> class that serves the dual role of both setting up your game as well as rendering your game content. Depending on the targeted platform, <code>CCGameView</code> inherits from a corresponding native view class as showcased below:

![CCGameView](https://raw.githubusercontent.com/mono/CocosSharp/forms_support/ReleaseNotes/FormsNotesContent/gameview_stack.png "Design of CCGameView")

The benefit of this design is that now a user simply treats <code>CCGameView</code> as they would any other native view. In particular, that means that on iOS, a <code>CCGameView</code> can now be specified within a <code>.xib</code>, while on Android we can make use of our resource <code>.axml</code> and similarly on WindowsPhone setup our view within a <code>.xaml</code>. For instance, a simple Android layout resource file for an activity with a game view could be 

<pre>
<code>
&lt; ?xml version="1.0" encoding="utf-8"? /&gt;
&lt;LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    android:orientation="vertical"
    android:layout_width="fill_parent"
    android:layout_height="fill_parent" &gt;
&lt;CocosSharp.CCGameView
     android:id="@+id/GameView"
     android:layout_width="fill_parent"
     android:layout_height="fill_parent" /&gt;
&lt;/LinearLayout&gt;
</code>
</pre>

Overall, we now have a much more natural and cohesive setup for your game, and importantly users can not only specify the dimensions of their view, but <em>when</em> their game view appears within their application.	



## Replacing CCApplicationDelegate with the ViewCreated event

As highlighted in the previous section, a <code>CCApplication</code> instance was paired with a corresponding <code>CCApplicaitonDelegate</code> that was in charge of loading your game content. For example,

<pre>
<code>
    public class GameAppDelegate : CCApplicationDelegate
    {
        public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
        {
            // Specify default world dimensions
            CCScene.SetDefaultDesignResolution(1024.f, 768.f, CCSceneResolutionPolicy.ShowAll);
            
            CCScene scene = new CCScene (mainWindow);
            
            // Setup your scene
            // ...
            
            mainWindow.RunWithScene (scene);
        }
    }
</code>
</pre>

With the introduction of the new <code>CCGameView</code> we have removed this cumbersome approach of subclassing <code>CCApplicationDelegate</code>, replacing it with the <code>CCGameView</code> event <code>ViewCreated</code>, which users hook up to in order to initialise their game content. So for example, on iOS, for a game view associated with a custom view controller we could do the following,

<pre>
<code>
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        CocosSharp.CCGameView GameView { get; set; }

        ...
</code>
</pre>  

and subsequently a user would then hook up to the <code>ViewCreated</code> event

<pre>
<code>
public partial class ViewController : UIViewController
{
    public override void ViewDidLoad ()
    {
        base.ViewDidLoad ();

        if (GameView != null) {
            // Set loading event to be called once game view is fully initialised
            GameView.ViewCreated += LoadGame;
        }
        
        // Called once the game view has been fully initialised
        void LoadGame (object sender, EventArgs e)
        {
            CCGameView gameView = sender as CCGameView;

            if (gameView != null) {
                
                // Set world dimensions
                gameView.DesignResolution = new CCSizeI (1024, 768);

                gameView.ContentManager.SearchPaths = new List<string> () { "Fonts", "Sounds", "Images" };

                CCScene gameScene = new CCScene (gameView);
                gameScene.AddLayer (new GameLayer ());
                gameView.RunWithScene (gameScene);
            }
        }
    }

    ....
</code>
</pre>

Similarly, for an Android app, connecting to <code>ViewCreated</code> would be performed during the corresponding activity's initialisation,

<pre>
<code>
    public class MainActivity : Activity
    {
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Get our game view from the layout resource,
            // and attach the view created event to it
            CCGameView gameView = (CCGameView)FindViewById (Resource.Id.GameView);
            
            if (gameView != null)
                gameView.ViewCreated += LoadGame;
        }

        void LoadGame (object sender, EventArgs e)
        {
            // Same as above
        }
        
    ....
</code>
</pre>

while finally on WindowsPhone, a custom <code>Page</code> with a xaml-specified game view would be setup within the constructor

<pre>
<code>
    public sealed partial class MainPage : Page
    {
        public MainPage ()
        {
            this.InitializeComponent ();

            if (GameView != null)
                GameView.ViewCreated += LoadGame;
        }

        void LoadGame (object sender, EventArgs e)
        {
            // Same as above
        }
    
    ....
</code>
</pre>


## CocosSharpView for Xamarin.Forms

We have also introduced the new custom Xamarin.Forms View <code>CocosSharpView</code>, whose custom view renderers are built on top of our <code>CCGameView</code> stack as highlighted below

![CocosSharpView](https://raw.githubusercontent.com/mono/CocosSharp/forms_support/ReleaseNotes/FormsNotesContent/forms_stack.png "Design of CocosSharpView")

Initialising a <code>CocosSharpView</code> is just as easy as creating any other Xamarin.Forms control:

<pre>
<code>
public class GamePage : ContentPage
{
    CocosSharpView gameView;

    public GamePage ()
    {
        gameView = new CocosSharpView () {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand,
            // Set the game world dimensions
            DesignResolution = new Size (1024, 768),
            // Set the method to call once the view has been initialised
            ViewCreated = LoadGame
        };

        Content = gameView;
    }

    void LoadGame (object sender, EventArgs e)
    {
        var nativeGameView = sender as CCGameView;

        if (nativeGameView != null) {
            // As in past examples
        }
    }

    ....

</code>
</pre>

## Limitations

Firstly, please be aware that for our initial prerelease, CocosSharp 1.7.0.0-pre1, we have reduced the scope of our package **to only target iOS, Android or Windows Phone 8.1 projects**. In subsequent releases, we will broaden the range of platforms supported.

More importantly, despite all the advances that are introduced by the new <code>CCGameView</code> class, there are still some constraints imposed by our dependencies. In particular, while a user can create, destroy and subsequently recreate multiple instances of a <code>CCGameView</code>, **currently we do not support the ability to have multiple <em>concurrent</em> <code>CCGameView</code> instances. In other words, for a given ViewController, Activity, Page etc., there can only be one <code>CCGameView</code> active at any point in time.**

## Will I necessarily need Xamarin.Forms to make use of these new features?

No. To make it perfectly clear &mdash; <code>CCGameView</code> is a native view implementation, meaning that you're free to incorporate this class within a native iOS, Android or WindowsPhone project. 

For Xamarin.Forms users, the benefit of using the Forms-specific <code>CocosSharpView</code> lies in the ability to specify all the UI elements of your game within a single, cross-platform project. 

## And remember...

as a prerelease we'll be constantly looking to fix and improve what is a substantial redesign &mdash; something that is only possible with your support and feedback.
