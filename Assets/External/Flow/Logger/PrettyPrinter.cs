using System;
using System.Linq;
using System.Text;
using Flow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Flow.Logger
{
    public class PrettyPrinter
    {
        public int Indenting = 4;

        public static string ToString(ITransient trans)
        {
            return new PrettyPrinter(trans)._sb.ToString();
        }

        public PrettyPrinter(ITransient trans)
        {
            Print(trans, 0);
        }

        private int Print(ITransient trans, int level)
        {
            if (trans == null)
                return level;

            Lead(level);
            Header(trans);
            var group = trans as IGroup;
            return group == null ? level : Contents(group, level + 1);
        }

        private void Lead(int level)
        {
            _sb.Append(' ', level*Indenting);
        }

        private StringBuilder Header(ITransient trans)
        {
            Assert.IsNotNull(trans);

            var name = trans.Name ?? "anon";
            var tyName = trans.GetType().Name;

            var ty = trans.GetType();
            if (ty == typeof(ITransient))
                return _sb.AppendFormat($"{tyName}={name}:\n");

            if (typeof(IFuture<>).IsAssignableFrom(ty))
            {
                var arg = ty.GetGenericArguments()[0];
                var avail = (bool)ty.GetProperty("Available")?.GetValue(trans);
                object val = "<unset>";
                if (avail)
                    val = ty.GetProperty("Value")?.GetValue(trans);
                return _sb.AppendFormat($"Future<{arg.Name}>: {name} Available={avail}, Value={val}\n");
            }

            if (typeof(IGroup).IsAssignableFrom(ty))
            {
                var g = (IGroup) trans;
                return _sb.AppendFormat($"Group: {name} NumChildren={g.Contents.Count()}");
            }

            return _sb.Append("??");
        }

        private int Contents(IGroup group, int level)
        {
            foreach (var tr in group.Contents)
            {
                Print(tr, level + 1);
            }
            return level;
        }

        private readonly StringBuilder _sb = new StringBuilder();
    }
}
