namespace KMLib
{
    public enum AltitudeMode
    {
        clampedToGround, 
        relativeToGround, 
        absolute
    }

    public enum RefreshMode
    {
        onChange, 
        onInterval, 
        onExpire
    }

    public enum ViewRefreshMode
    {
        never, 
        onStop, 
        onRequest, 
        onRegion
    }
}