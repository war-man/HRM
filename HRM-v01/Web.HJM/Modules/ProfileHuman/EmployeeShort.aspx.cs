﻿using Ext.Net;
using SoftCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Core.Framework;
using Web.Core.Object.Catalog;
using Web.Core.Object.HumanRecord;
using Web.Core.Object.Security;

namespace Web.HJM.Modules.ProfileHuman
{
    public partial class EmployeeShort : BasePage
    {
        string generateEmployeeConst = "00000000000000000000";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ExtNet.IsAjaxRequest)
            {
                // init department
                storeDepartment.DataSource = CurrentUser.DepartmentsTree;
                storeDepartment.DataBind();

                // init employee type
                storeEmployeeType.DataSource = CatalogController.GetAll("cat_EmployeeType", null, null, null, false, null, null);
                storeEmployeeType.DataBind();

                //init generate employeeCode
                txtEmployeeCode.Text = GenerateEmployeeCode();
                wdEmployeeShort.Show();
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            try
            {
                var util = new Util();
                var hoso = new RecordController();
                var hs = new hr_Record();
                
                hs.EmployeeCode = txtEmployeeCode.Text;
                if (!string.IsNullOrEmpty(txtEmployeeCode.Text.Trim()))
                {
                    List<RecordModel> recordList = RecordController.CheckExistIDNumberAndEmployeeCode(null, null, txtEmployeeCode.Text);
                    if (recordList != null && recordList.Count > 0)
                    {
                        Dialog.ShowError("Mã cán bộ đã tồn tại. Vui lòng nhập mã cán bộ khác.");
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(txtIDNumber.Text.Trim()))
                {
                    List<RecordModel> recordList = RecordController.CheckExistIDNumberAndEmployeeCode(null, txtIDNumber.Text, null);
                    if (recordList != null && recordList.Count > 0)
                    {
                        Dialog.ShowError("Số chứng minh nhân dân đã tồn tại. Vui lòng nhập số chứng minh nhân dân khác.");
                        return;
                    }
                }
                
                hs.FullName = txtFullName.Text;
                if (!string.IsNullOrEmpty(hdfEmployeeTypeId.Text))
                    hs.EmployeeTypeId = Convert.ToInt32(hdfEmployeeTypeId.Text);
                if (!string.IsNullOrEmpty(hdfDepartmentId.Text))
                    hs.DepartmentId = Convert.ToInt32(hdfDepartmentId.Text);
                if (!string.IsNullOrEmpty(hdfManagementDepartmentId.Text))
                    hs.ManagementDepartmentId = Convert.ToInt32(hdfManagementDepartmentId.Text);
                // lấy họ và đệm từ họ tên
                var position = hs.FullName.LastIndexOf(' ');
                if (position == -1)
                {
                    hs.Name = hs.FullName;
                }
                else
                {
                    hs.Name = hs.FullName.Substring(position + 1).Trim();
                }
                if (!util.IsDateNull(dfBirthDate.SelectedDate))
                    hs.BirthDate = dfBirthDate.SelectedDate;
                if (!string.IsNullOrEmpty(cbxSex.SelectedItem.Value) && cbxSex.SelectedItem.Value == "M")
                {
                    hs.Sex = true;
                }
                else
                {
                    hs.Sex = false;
                }
                hs.Address = txt_Address.Text;
                if (!string.IsNullOrEmpty(txtIDNumber.Text))
                    hs.IDNumber = txtIDNumber.Text;
                hs.PersonalEmail = txtPersonalEmail.Text;
                hs.CellPhoneNumber = txtCellPhoneNumber.Text;

                RecordController.InsertRecord(hs);
                Dialog.ShowNotification("Thêm mới thành công!");
                ClearForm();
            }
            catch (Exception ex)
            {
                Dialog.ShowError(ex.Message);
            }
        }

        private void ClearForm()
        {
            txtFullName.Reset();
            txtEmployeeCode.Text = "";
            txtManagementDepartment.Reset();
            cbxSex.Clear();
            txtIDNumber.Reset();
            txtCellPhoneNumber.Reset();
            txtPersonalEmail.Reset();
            txt_Address.Reset();
            hdfDepartmentId.Reset();
            hdfEmployeeTypeId.Reset();
            hdfManagementDepartmentId.Reset();
            cboDepartment.Clear();
            cboEmployeeType.Clear();
            dfBirthDate.Reset();
        }

        /// <summary>
        /// Sinh mã cán bộ dựa vào cấu hình hệ thống:
        /// -   Tiền tố của mã cán bộ
        /// -   Số lượng chữ số theo sau tiền tố
        /// </summary>
        /// <returns>Mã cán bộ mới được sinh ra</returns>
        public string GenerateEmployeeCode()
        {
            string departments = string.Join(",", CurrentUser.Departments.Select(d => d.Id));
            string prefix = SystemConfigController.GetValueByNameFollowDepartment(SystemConfigParameter.PREFIX, departments);
            string numberCharacter = SystemConfigController.GetValueByNameFollowDepartment(SystemConfigParameter.NUMBER_OF_CHARACTER, departments);
            if (string.IsNullOrEmpty(prefix))
                prefix = "CB";
            int number = string.IsNullOrEmpty(numberCharacter) ? 5 : int.Parse(numberCharacter);

            var record = RecordController.GetByEmployeeCodeGenerate(prefix, number);
            var oldEmployeeCode = generateEmployeeConst;
            if (record != null && !string.IsNullOrEmpty(record.EmployeeCode))
                oldEmployeeCode = record.EmployeeCode;
            long oldNumber = long.Parse("" + oldEmployeeCode.Substring(prefix.Length));
            oldNumber++;
            string newEmployeeCode = generateEmployeeConst + oldNumber;
            newEmployeeCode = prefix + newEmployeeCode.Substring(newEmployeeCode.Length - number);
            return newEmployeeCode;
        }

        [DirectMethod]
        public void ChangeDepartment(int id)
        {
            // init parent name
            var managementDepartmentName = "";
            // find current department
            var currentDepartment = CurrentUser.Departments.FirstOrDefault(d => d.Id == id);
            // check current department
            if (currentDepartment != null)
            {
                var parentDepartment = CurrentUser.Departments.FirstOrDefault(d => d.Id == currentDepartment.ParentId);
                while (parentDepartment != null)
                {
                    // set parent department name
                    managementDepartmentName = parentDepartment.Name;
                    // set parent department id
                    hdfManagementDepartmentId.Text = parentDepartment.Id.ToString();
                    // check parent department type
                    if (parentDepartment.Type == DepartmentType.Organization)
                        // is organization, break
                        break;
                    // is board or department, move up
                    parentDepartment = CurrentUser.Departments.FirstOrDefault(d => d.Id == parentDepartment.ParentId);
                }
                // set display text field
                txtManagementDepartment.SetValue(managementDepartmentName);
            }
        }
    }
}
