namespace BiblioTech.API.Models.BaseDTO
{
    public abstract record BaseUpdateEntityDTO
    {
        protected BaseUpdateEntityDTO() {
            ModifyDate = DateTime.Now;
        }
        public DateTime ModifyDate { get; set; }
        public int ModifyUser {  get; set; }
        public int CreationUser { get; set; }
    }
}
