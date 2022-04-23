namespace BattleShip
{
    public class BoardIndex
    {
        public int RowCheckStartIndex { get; set; } = -1;
        public int ColumnCheckStartIndex { get; set; } = -1;
        public int RowCheckEndIndex { get; set; } = 1;
        public int ColumnCheckEndIndex { get; set; } = 1;
    }
}
