
namespace CocosSharp
{
    public class CCIMEKeyboardNotificationInfo : System.EventArgs
    {
		public CCRect Begin;              	// the soft keyboard rectangle when animatin begin
		public CCRect End;                	// the soft keyboard rectangle when animatin end
		public float Duration;           	// the soft keyboard animation duration
    }
}
