namespace TTDN.Models
{
    public class FieldScheduleViewModel
    {
        public int ScheduleId { get; set; }
        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Status { get; set; }
        public decimal Price1 { get; set; }
    }
}
