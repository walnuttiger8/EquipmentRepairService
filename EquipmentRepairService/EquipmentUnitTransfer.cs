//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EquipmentRepairService
{
    using System;
    using System.Collections.Generic;
    
    public partial class EquipmentUnitTransfer
    {
        public int Id { get; set; }
        public Nullable<int> EquipmentUnitId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public System.DateTime TransferDate { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual EquipmentUnit EquipmentUnit { get; set; }
    }
}
