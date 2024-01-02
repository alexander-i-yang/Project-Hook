using System;

namespace UI
{
    public class DevConsole : ASK.UI.DevConsole
    {
        public override string[] Commands() => new[] { "timescale" };

        public override Action<string>[] CommandActions() => new Action<string>[] { TimeScale };
    }
}