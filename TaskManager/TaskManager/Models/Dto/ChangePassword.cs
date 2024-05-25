namespace TaskManager.Models.Dto
{
    public class ChangePassword
    {
        /// <summary>
        /// شناسه کاربر
        /// </summary>                                                                                                                                 
        public int Id { get; set; }
        
        /// <summary>
        ///گذرواژه قبلی
        /// </summary> 
        public string OldPassword { get; set; }
        
        /// <summary>
        ///گذرواژه جدید
        /// </summary> 
        public string NewPassword { get; set; }
    }
}
