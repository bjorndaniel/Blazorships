using Blazorships.Shared.Boards;
using System.Collections.Generic;
using System.Linq;

namespace Blazorships.Shared.Helpers
{
    public static class PanelExtensions
    {
        public static Panel At(this List<Panel> panels, int row, int column) =>
            panels.Where(x => x.Coordinates.Row == row && x.Coordinates.Column == column).First();

        public static List<Panel> Range(this List<Panel> panels, int startRow, int startColumn, int endRow, int endColumn)
        {
            return panels.Where(x => x.Coordinates.Row >= startRow
                && x.Coordinates.Column >= startColumn
                && x.Coordinates.Row <= endRow
                && x.Coordinates.Column <= endColumn).ToList();
        }
    }
}
