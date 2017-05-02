namespace Emgu.CV.Dpm
{
    /// <summary>
    /// Provide interfaces to the Open CV DPM functions
    /// </summary>
    public static partial class DpmInvoke
    {
        static DpmInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }
    }
}
