namespace AcademiaBackEnd.Request.Professor
{
    public class RemoveWorkoutRequest : Request
    {
        public long userclientId {  get; set; }

        public long workoutId { get; set; }
    }
}
