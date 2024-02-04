namespace BlApi;

static class Factory
{
    public static IBl Get() => new BlImplementation.Bl();
}
