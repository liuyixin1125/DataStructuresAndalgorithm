using System;
namespace pllllllll.Properties
{
    public class Person
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public bool Sex { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public Person()
        {
            this.ID = Guid.NewGuid();
        }
    }
}
