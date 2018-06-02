using System;

namespace App.View
{
    using Registry;

    public class ViewRegistry
        : Registry<IViewBase>
    {
        public override IViewBase Prepare(IViewBase model)
        {
            return base.Prepare(model);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
