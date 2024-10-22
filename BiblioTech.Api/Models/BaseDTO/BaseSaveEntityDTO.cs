namespace BiblioTech.API.Models.BaseDTO
{
    public abstract record BaseSaveEntityDTO
    {
        protected BaseSaveEntityDTO() {
        CreationDate = DateTime.Now;
        
        }

        public int CreationUser {  get; set; }
        public DateTime CreationDate { get; set; }
    }
}
