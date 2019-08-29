﻿using System;

namespace Web.Core.Object.HumanRecord
{
    public class hr_Reward : BaseEntity//HOSO_KHENTHUONG
    {
        /// <summary>
        /// id hồ sơ
        /// </summary>        
        public int RecordId { get; set; } //FR_KEY
        /// <summary>
        /// So quyet dinh
        /// </summary>        
        public string DecisionNumber { get; set; }//SO_QD
        /// <summary>
        /// Ngay quyet dinh
        /// </summary>
        public DateTime? DecisionDate { get; set; }//NGAY_QD
        /// <summary>
        /// Ngay hieu luc
        /// </summary>
        public DateTime? EffectiveDate { get; set; }//NGAY_HIEU_LUC
        /// <summary>
        /// Ly do khen thuong
        /// </summary>
        public string Reason { get; set; }//LYDO_KTHUONG
        /// <summary>
        /// Id hinh thuc khen thuong
        /// </summary>
        public int FormRewardId { get; set; }//MA_HT_KTHUONG
        /// <summary>
        /// So tien
        /// </summary>        
        public Decimal MoneyAmount { get; set; }//SO_TIEN
        /// <summary>
        /// Ghi chu
        /// </summary>
        public string Note { get; set; }//GHI_CHU
        /// <summary>
        /// Nguoi quyet dinh
        /// </summary>
        public string DecisionMaker { get; set; }//NguoiQuyetDinh
        /// <summary>
        /// Ten file dinh kem
        /// </summary>
        public string AttachFileName { get; set; }//TepTinDinhKem
        /// <summary>
        /// Duyet
        /// </summary>
        public bool IsApproved { get; set; }//Duyet
        /// <summary>
        /// So diem
        /// </summary>
        public double Point { get; set; }//SoDiem
        /// <summary>
        /// Id cap khen thuong ky luat
        /// </summary>
        public int LevelRewardId { get; set; }//MA_CAPKHENTHUONGKYLUAT
        /// <summary>
        /// Chức vụ người ký
        /// </summary>
        public string MakerPosition { get; set; }
        /// <summary>
        /// Cơ quan khen thưởng
        /// </summary>
        public string SourceDepartment { get; set; }

        // các trường mặc định
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
    }
}