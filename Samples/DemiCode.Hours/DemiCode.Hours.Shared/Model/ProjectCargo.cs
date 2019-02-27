namespace DemiCode.Hours.Shared.Model
{
    public class ProjectCargo
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public EmployeeCargo Manager { get; set; }
    }
}
