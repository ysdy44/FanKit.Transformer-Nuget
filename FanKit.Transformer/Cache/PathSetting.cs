using System.Collections.Generic;

namespace FanKit.Transformer.Cache
{
    public class PathSetting
    {
        CheckState Mode;

        public SelectedMode SelectedMode
        {
            get
            {
                switch (Mode)
                {
                    case CheckState.Empty: return SelectedMode.Empty;
                    case CheckState.AllUnchecked: return SelectedMode.UnSelected;
                    case CheckState.AllChecked: return SelectedMode.Selected;
                    case CheckState.Indeterminate: return SelectedMode.Indeterminate;
                    default: return SelectedMode.Empty;
                }
            }
        }

        public RemoveMode RemoveMode
        {
            get
            {
                switch (Mode)
                {
                    case CheckState.Empty: return RemoveMode.NoRemove;
                    case CheckState.AllUnchecked: return RemoveMode.NoRemove;
                    case CheckState.AllChecked: return RemoveMode.RemoveCurve;
                    case CheckState.Indeterminate: return RemoveMode.RemoveNodes;
                    default: return RemoveMode.NoRemove;
                }
            }
        }

        public void UpdateSelectedMode(IGetFigure figure)
        {
            int checks = figure.GetChecksCount();
            int unChecks = figure.Count - checks;

            switch (unChecks)
            {
                case 0:
                    switch (checks)
                    {
                        case 0: Mode = CheckState.Empty; break;
                        default: Mode = CheckState.AllChecked; break;
                    }
                    break;
                //case 1: this.Mode =  VexsCheckState.AllChecked; break;
                default:
                    switch (checks)
                    {
                        case 0: Mode = CheckState.AllUnchecked; break;
                        default: Mode = CheckState.Indeterminate; break;
                    }
                    break;
            }
        }

        public void UpdateRemoveMode(IGetFigure figure)
        {
            int checks = figure.GetChecksCount();
            int unChecks = figure.Count - checks;

            switch (unChecks)
            {
                case 0:
                    switch (checks)
                    {
                        case 0: Mode = CheckState.Empty; break;
                        default: Mode = CheckState.AllChecked; break;
                    }
                    break;
                case 1: Mode = CheckState.AllChecked; break;
                default:
                    switch (checks)
                    {
                        case 0: Mode = CheckState.AllUnchecked; break;
                        default: Mode = CheckState.Indeterminate; break;
                    }
                    break;
            }
        }

        public void UpdateSelectedMode(IEnumerable<IGetFigure> figures)
        {
            Mode = 0;

            foreach (IGetFigure item in figures)
            {
                item.Setting.UpdateSelectedMode(item);

                Mode |= item.Setting.Mode;
            }
        }

        public void UpdateRemoveMode(IEnumerable<IGetFigure> figures)
        {
            Mode = 0;

            foreach (IGetFigure item in figures)
            {
                item.Setting.UpdateRemoveMode(item);

                Mode |= item.Setting.Mode;
            }
        }
    }
}