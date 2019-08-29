﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="Web.HJM.Modules.Salary.SalaryColumnDynamic" Codebehind="SalaryColumnDynamic.aspx.cs" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="~/Modules/ChooseEmployee/ucChooseEmployee.ascx" TagPrefix="uc1"
    TagName="ucChooseEmployee" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../Resource/js/Extcommon.js" type="text/javascript"></script>
    <script src="../../Resource/js/global.js" type="text/javascript"></script>
    <script src="../../Resource/js/RenderJS.js" type="text/javascript"></script>
    <style type="text/css">
        div#gridColumnDynamic .x-grid3-cell-inner, .x-grid3-hd-inner {
            white-space: nowrap !important;
        }

        #pnReportPanel .x-tab-panel-header {
            display: none !important;
        }
    </style>

    <script type="text/javascript">

        var keyPresstxtSearch = function (f, e) {
            if (e.getKey() == e.ENTER) {
                PagingToolbar1.pageIndex = 0;
                PagingToolbar1.doLoad();
                RowSelectionModel1.clearSelections();
                if (this.getValue() == '') {
                    this.triggers[0].hide();
                }
            }
            if (this.getValue() != '') {
                this.triggers[0].show();
            }
        }
        var ResetwdColumnDynamic = function () {

            grp_ListEmployeeStore.removeAll();
        }
        var ReloadGrid = function () {
            PagingToolbar1.pageIndex = 0;
            PagingToolbar1.doLoad();
            RowSelectionModel1.clearSelections();
        }
        var triggershowChooseEmplyee = function (f, e) {
            if (e.getKey() == e.ENTER) {
                ucChooseEmployee1_wdChooseUser.show();
            }
        }
        var CheckInputHL = function () {
            if (grp_ListEmployee.store.getCount() == 0) {
                alert('Bạn chưa chọn cán bộ nào!');
                return false;
            }
            if (cbxColumnCode.getValue() == '' || cbxColumnCode.getValue() == null) {
                alert('Bạn chưa chọn mã cột');
                return false;
            }

            return true;
        }

        var CheckInput = function () {
            if (txtUpdateColumnCode.getValue() == '' || txtUpdateColumnCode.getValue() == null) {
                alert('Bạn chưa nhập mã cột');
                return false;
            }

            return true;
        }

        var addRecord = function (RecordId, EmployeeCode, FullName, DepartmentName) {
            var rowindex = getSelectedIndexRow();
            grp_ListEmployee.insertRecord(rowindex, {
                RecordId: RecordId,
                EmployeeCode: EmployeeCode,
                FullName: FullName,
                DepartmentName: DepartmentName
            });
            grp_ListEmployee.getView().refresh();
            grp_ListEmployee.getSelectionModel().selectRow(rowindex);
        }
        var getSelectedIndexRow = function () {
            var record = grp_ListEmployee.getSelectionModel().getSelected();
            var index = grp_ListEmployee.store.indexOf(record);
            if (index == -1)
                return 0;
            return index;
        }

        var getPrKeyRecordList = function () {
            var jsonDataEncode = "";
            var records = window.grp_ListEmployeeStore.getRange();
            for (var i = 0; i < records.length; i++) {
                jsonDataEncode += records[i].data.RecordId + ",";
            }
            return jsonDataEncode;
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager ID="RM" runat="server">
            </ext:ResourceManager>
            <ext:Hidden runat="server" ID="hdfId" />
            <ext:Hidden runat="server" ID="hdfRecordId" />
            <ext:Hidden runat="server" ID="hdfDepartmentSelected" />
            <ext:Hidden runat="server" ID="hdfMenuID" />
            <ext:Hidden runat="server" ID="hdfTotalRecord" />
            <ext:Hidden runat="server" ID="hdfDepartments" />
            <ext:Hidden runat="server" ID="hdfIsLocked" />
            <ext:Hidden runat="server" ID="hdfConfigId" />
           

            <uc1:ucChooseEmployee runat="server" ID="ucChooseEmployee" ChiChonMotCanBo="false"
                DisplayWorkingStatus="TatCa" />
            <ext:Viewport ID="vp" runat="server" HideBorders="true">
                <Items>
                    <ext:BorderLayout runat="server" ID="brlayout">
                        <Center>
                            <ext:GridPanel ID="gridColumnDynamic" TrackMouseOver="true" runat="server"
                                StripeRows="true" Border="false" AnchorHorizontal="100%" Title="Vui lòng chọn bảng lương để cấu hình" Icon="Date">
                                <TopBar>
                                    <ext:Toolbar runat="server" ID="toolbarFn">
                                        <Items>
                                            <ext:Button ID="btnSalaryBoardManager" runat="server" Text="Cấu hình cột động bảng lương" Icon="Table" Hidden="true">
                                                <Listeners>
                                                    <Click Handler="wdSalaryBoardManage.show();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button ID="btnBack" runat="server" Text="Quay lại" Icon="ArrowLeft">
                                                <DirectEvents>
                                                    <Click OnEvent="btnBack_Click">
                                                        <EventMask ShowMask="true" Msg="Đang tải..." />
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator />
                                            <ext:Button runat="server" ID="btnAdd" Text="Thêm" Icon="Add">
                                                <Listeners>
                                                    <Click Handler="wdColumnDynamic.show();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server" ID="btnEdit" Text="Sửa" Icon="Pencil" Disabled="true">
                                                <Listeners>
                                                    <Click Handler="if (CheckSelectedRows(gridColumnDynamic) == false) {return false;}; " />
                                                </Listeners>
                                                <DirectEvents>
                                                    <Click OnEvent="BtnEdit_Click">
                                                        <EventMask ShowMask="true" />
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                            <ext:Button runat="server" ID="btnDelete" Text="Xóa" Icon="Delete" Disabled="true">
                                                <Listeners>
                                                    <Click Handler="return CheckSelectedRow(gridColumnDynamic);" />
                                                </Listeners>
                                                <DirectEvents>
                                                    <Click OnEvent="BtnDelete_Click">
                                                        <Confirmation Title="Thông báo từ hệ thống" Message="Bạn có chắc chắn muốn xóa?"
                                                            ConfirmRequest="true" />
                                                        <EventMask ShowMask="true" />
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                            <ext:ToolbarSpacer Width="5" />
                                            <ext:Button ID="btnImportExcel" runat="server" Text="Nhập từ excel" Icon="PageExcel">
                                                <Listeners>
                                                    <Click Handler="#{wdImportExcelFile}.show();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ToolbarSeparator />
                                            <ext:ToolbarSpacer Width="10" />

                                            <ext:ToolbarFill />
                                            <ext:TriggerField runat="server" ID="txtSearch" EnableKeyEvents="true" Width="270"
                                                EmptyText="Nhập tên, mã nhân viên, mã cột hoặc tên cột">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                                </Triggers>
                                                <Listeners>
                                                    <KeyPress Fn="keyPresstxtSearch" />
                                                    <TriggerClick Handler="this.triggers[0].hide(); this.clear();#{PagingToolbar1}.pageIndex = 0; #{PagingToolbar1}.doLoad(); RowSelectionModel1.clearSelections();" />
                                                </Listeners>
                                            </ext:TriggerField>
                                            <ext:Button runat="server" ID="btnSearch" Text="Tìm kiếm" Icon="Zoom">
                                                <Listeners>
                                                    <Click Handler="#{PagingToolbar1}.pageIndex = 0; #{PagingToolbar1}.doLoad(); RowSelectionModel1.clearSelections();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store runat="server" ID="storeAdjustment" GroupField="FullName">
                                        <Proxy>
                                            <ext:HttpProxy Method="POST" Url="~/Services/BaseHandler.ashx" />
                                        </Proxy>
                                        <AutoLoadParams>
                                            <ext:Parameter Name="start" Value="={0}" />
                                            <ext:Parameter Name="limit" Value="={25}" />
                                        </AutoLoadParams>
                                        <BaseParams>
                                            <ext:Parameter Name="handlers" Value="SalaryColumnDynamic" />
                                            <ext:Parameter Name="departmentSelected" Value="hdfDepartmentSelected.getValue()" Mode="Raw" />
                                            <ext:Parameter Name="SearchKey" Value="txtSearch.getValue()" Mode="Raw" />
                                            <ext:Parameter Name="departments" Value="hdfDepartments.getValue()" Mode="Raw" />
                                            <ext:Parameter Name="salaryBoardListID" Value="hdfSalaryBoardListID.getValue()" Mode="Raw" />
                                        </BaseParams>
                                        <Reader>
                                            <ext:JsonReader IDProperty="Id" Root="Data" TotalProperty="TotalRecords">
                                                <Fields>
                                                    <ext:RecordField Name="Id" />
                                                    <ext:RecordField Name="RecordId" />
                                                    <ext:RecordField Name="EmployeeCode" />
                                                    <ext:RecordField Name="FullName" />
                                                    <ext:RecordField Name="DepartmentName" />
                                                    <ext:RecordField Name="ColumnCode" />
                                                    <ext:RecordField Name="Display" />
                                                    <ext:RecordField Name="Value" />
                                                    <ext:RecordField Name="ColumnExcel" />
                                                    <ext:RecordField Name="IsInUsed" />
                                                    <ext:RecordField Name="CreatedDate" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                        <Listeners>
                                            <Load Handler="#{RowSelectionModel1}.clearSelections();" />
                                        </Listeners>
                                    </ext:Store>
                                </Store>
                                <View>
                                    <ext:GroupingView ID="GroupingView1" runat="server" ForceFit="false" MarkDirty="false"
                                        ShowGroupName="false" EnableNoGroups="true" HideGroupedColumn="true" />
                                </View>
                                <ColumnModel>
                                    <Columns>
                                        <ext:RowNumbererColumn Header="STT" Width="35" Locked="true" />
                                        <ext:Column Header="Mã nhân viên" Width="85" Align="Left" Locked="true" DataIndex="EmployeeCode" />
                                        <ext:GroupingSummaryColumn ColumnID="FullName" DataIndex="FullName" Header="Họ tên" Width="200" Sortable="true" Hideable="false" SummaryType="Count" />
                                        <ext:Column Header="Phòng ban" Width="250" Align="Left" DataIndex="DepartmentName" />
                                        <ext:Column Header="Mã cột" Width="250" Align="Left" DataIndex="ColumnCode" />
                                        <ext:Column Header="Tên cột" Width="250" Align="Left" DataIndex="Display" />
                                        <ext:Column Header="Cột Excel" Width="100" Align="Left" DataIndex="ColumnExcel" Hidden="True" />
                                        <ext:Column Header="Giá trị" Width="250" Align="Left" DataIndex="Value" />
                                        <ext:Column Header="Đang sử dụng" Width="100" Align="Center" DataIndex="IsInUsed" Hidden="True">
                                            <Renderer Fn="GetBooleanIcon" />
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <LoadMask ShowMask="true" Msg="Đang tải...." />
                                <SelectionModel>
                                    <ext:RowSelectionModel runat="server" ID="RowSelectionModel1">
                                        <Listeners>
                                            <RowSelect Handler="hdfId.setValue(RowSelectionModel1.getSelected().id);hdfRecordId.setValue(RowSelectionModel1.getSelected().data.RecordId);" />
                                            <RowDeselect Handler="hdfId.reset();hdfRecordId.reset();" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <DirectEvents>
                                    <RowDblClick>
                                        <EventMask ShowMask="true" />
                                    </RowDblClick>
                                </DirectEvents>
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="25">
                                        <Items>
                                            <ext:Label ID="Label1" runat="server" Text="Số bản ghi trên một trang:" />
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                            <ext:ComboBox ID="ComboBox1" runat="server" Width="80">
                                                <Items>
                                                    <ext:ListItem Text="20" />
                                                    <ext:ListItem Text="25" />
                                                    <ext:ListItem Text="30" />
                                                    <ext:ListItem Text="40" />
                                                    <ext:ListItem Text="50" />
                                                    <ext:ListItem Text="100" />
                                                </Items>
                                                <SelectedItem Value="25" />
                                                <Listeners>
                                                    <Select Handler="#{PagingToolbar1}.pageSize = parseInt(this.getValue()); #{PagingToolbar1}.doLoad();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:PagingToolbar>
                                </BottomBar>
                            </ext:GridPanel>
                        </Center>
                    </ext:BorderLayout>
                </Items>
            </ext:Viewport>
            <ext:Window runat="server" ID="wdColumnDynamic" Constrain="true" Modal="true" Title="Tạo mới cấu hình cột động"
                Icon="UserAdd" Layout="FormLayout" Resizable="false" AutoHeight="true" Width="650"
                Hidden="true" Padding="6" LabelWidth="120">
                <Items>
                    <ext:Container ID="Container7" runat="server" Layout="ColumnLayout" Height="70">
                        <Items>
                            <ext:Container ID="Container8" runat="server" LabelAlign="left" Layout="Form" ColumnWidth=".5"
                                LabelWidth="120">
                                <Items>
                                    <ext:Hidden ID="hdfColumnCode" runat="server" />
                                    <ext:ComboBox runat="server" ID="cbxColumnCode" FieldLabel="Mã cột" AnchorHorizontal="98%" 
                                        DisplayField="ColumnCode" ValueField="ColumnCode" ItemSelector="div.list-item" PageSize="20">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                        </Triggers>
                                        <Template ID="Template19" runat="server">
                                            <Html>
                                                <tpl for=".">
						                            <div class="list-item"> 
							                            {ColumnCode}
						                            </div>
					                            </tpl>
                                            </Html>
                                        </Template>
                                        <Store>
                                            <ext:Store runat="server" ID="cbxColumnCodeStore" AutoLoad="true">
                                                <Proxy>
                                                    <ext:HttpProxy Method="POST" Url="~/Services/BaseHandler.ashx" />
                                                </Proxy>
                                                <BaseParams>
                                                    <ext:Parameter Name="handlers" Value="SalaryConfig" />
                                                    <ext:Parameter Name="ConfigId" Value="hdfConfigId.getValue()" Mode="Raw" />
                                                </BaseParams>
                                                <Reader>
                                                    <ext:JsonReader IDProperty="Id" Root="Data" TotalProperty="TotalRecords">
                                                        <Fields>
                                                            <ext:RecordField Name="Id" />
                                                            <ext:RecordField Name="ColumnCode" />
                                                            <ext:RecordField Name="Display" />
                                                            <ext:RecordField Name="Formula" />
                                                            <ext:RecordField Name="ColumnExcel" />
                                                            <ext:RecordField Name="IsReadOnly" />
                                                            <ext:RecordField Name="IsInUsed" />
                                                            <ext:RecordField Name="IsDisable" />
                                                            <ext:RecordField Name="Order" />
                                                            <ext:RecordField Name="Description" />
                                                            <ext:RecordField Name="DataTimeSheetHandlerType" />
                                                            <ext:RecordField Name="ConfigId" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Expand Handler="cbxColumnCodeStore.reload();"></Expand>
                                            <Select Handler="this.triggers[0].show();hdfColumnCode.setValue(cbxColumnCode.getValue());Ext.net.DirectMethods.SetColumnName()" />
                                            <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); hdfColumnCode.reset();txtDisplay.reset()}" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:TextField runat="server" ID="txtColumnExcel" FieldLabel="Cột Excel" AnchorHorizontal="98%" CtCls="requiredDate" Hidden="True"></ext:TextField>
                                    <ext:Checkbox runat="server" BoxLabel="Đang sử dụng" ID="chk_IsInUsed" Hidden="True" />
                                    <ext:TextField runat="server" ID="txtValue" FieldLabel="Giá trị" AnchorHorizontal="98%"></ext:TextField>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container1" runat="server" LabelAlign="left" Layout="Form" ColumnWidth=".5"
                                LabelWidth="120">
                                <Items>
                                    <ext:TextField runat="server" ID="txtDisplay" FieldLabel="Tên cột" AnchorHorizontal="98%" CtCls="requiredDate" ReadOnly="true"></ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>

                    <ext:Container runat="server" ID="ctn23" Layout="BorderLayout" Height="230">
                        <Items>
                            <ext:GridPanel runat="server" ID="grp_ListEmployee" TrackMouseOver="true" Title="Danh sách cán bộ"
                                StripeRows="true" Border="true" Region="Center" Icon="User" AutoExpandColumn="DepartmentName"
                                AutoExpandMin="150">
                                <TopBar>
                                    <ext:Toolbar runat="server" ID="tbListEmployee">
                                        <Items>
                                            <ext:Button runat="server" ID="btnChooseEmployee" Icon="UserAdd" Text="Chọn cán bộ"
                                                TabIndex="12">
                                                <Listeners>
                                                    <Click Handler="ucChooseEmployee_wdChooseUser.show();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server" ID="btnDeleteEmployee" Icon="Delete" Text="Xóa" Disabled="true"
                                                TabIndex="13">
                                                <Listeners>
                                                    <Click Handler="#{grp_ListEmployee}.deleteSelected(); #{hdfTotalRecord}.setValue(#{hdfTotalRecord}.getValue()*1 - 1);if(hdfTotalRecord.getValue() ==0){btnDeleteEmployee.disable(); btnAdjust.disable();}" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="grp_ListEmployeeStore" AutoLoad="false" runat="server" ShowWarningOnFailure="false"
                                        SkipIdForNewRecords="false" RefreshAfterSaving="None" AutoSave="false">
                                        <Reader>
                                            <ext:JsonReader IDProperty="RecordId">
                                                <Fields>
                                                    <ext:RecordField Name="RecordId" />
                                                    <ext:RecordField Name="EmployeeCode" />
                                                    <ext:RecordField Name="FullName" />
                                                    <ext:RecordField Name="DepartmentName" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel ID="ColumnModel3" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn Header="STT" Width="40" />
                                        <ext:Column ColumnID="EmployeeCode" Header="Mã cán bộ" Width="100" DataIndex="EmployeeCode" />
                                        <ext:Column ColumnID="FullName" Header="Họ tên" Width="200" DataIndex="FullName" />
                                        <ext:Column ColumnID="DepartmentName" Header="Bộ phận" Width="100" DataIndex="DepartmentName">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <View>
                                    <ext:LockingGridView runat="server" ID="lkv1" />
                                </View>
                                <SelectionModel>
                                    <ext:RowSelectionModel runat="server" ID="chkEmployeeRowSelection">
                                        <Listeners>
                                            <RowSelect Handler="btnDeleteEmployee.enable();" />
                                            <RowDeselect Handler="btnDeleteEmployee.disable();" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                            </ext:GridPanel>
                        </Items>
                    </ext:Container>
                </Items>
                <Buttons>
                    <ext:Button runat="server" ID="btnUpdateHL" Text="Cập nhật" Icon="Disk">
                        <Listeners>
                            <Click Handler=" return CheckInputHL();" />
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="BtnUpdateHL_Click">
                                <EventMask ShowMask="true" Msg="Đang lưu dữ liệu" />
                                <ExtraParams>
                                    <ext:Parameter Name="ListRecordId" Value="getPrKeyRecordList()" Mode="Raw" />
                                    <ext:Parameter Name="Close" Value="False" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button runat="server" ID="btnCloseHL" Text="Đóng lại" Icon="Decline" Hidden="True">
                        <Listeners>
                            <Click Handler="wdColumnDynamic.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Hide Handler="ResetwdColumnDynamic();" />
                </Listeners>
            </ext:Window>
            <ext:Window runat="server" ID="wdUpdateColumnDynamic" Constrain="true" Modal="true" Title="Cập nhật thông tin cấu hình cột động"
                Icon="UserAdd" Layout="FormLayout" Resizable="false" AutoHeight="true" Width="650"
                Hidden="true" Padding="6" LabelWidth="120">
                <Items>
                    <ext:Container ID="Container9" runat="server" Layout="ColumnLayout" Height="150">
                        <Items>
                            <ext:Container ID="Container10" runat="server" LabelAlign="left" Layout="Form" ColumnWidth=".5"
                                LabelWidth="120">
                                <Items>
                                    <ext:TextField runat="server" ID="txtFullName" FieldLabel="Họ và tên" AnchorHorizontal="98%" ReadOnly="true" />
                                    <ext:TextField runat="server" ID="txtUpdateColumnCode" FieldLabel="Mã cột" AnchorHorizontal="98%" CtCls="requiredDate" ReadOnly="true"></ext:TextField>
                                    <ext:TextField runat="server" ID="txtUpdateColumnExcel" FieldLabel="Cột Excel" AnchorHorizontal="98%" CtCls="requiredDate" Hidden="True"></ext:TextField>
                                    <ext:Checkbox runat="server" BoxLabel="Đang sử dụng" ID="chk_IsInUsedUpdate" Hidden="True" />
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Container11" runat="server" LabelAlign="left" Layout="Form" ColumnWidth=".5"
                                LabelWidth="120">
                                <Items>
                                    <ext:TextField runat="server" ID="txtEmployeeCode" FieldLabel="Mã nhân viên" AnchorHorizontal="98%" ReadOnly="true" />
                                    <ext:TextField runat="server" ID="txtUpdateDisplay" FieldLabel="Tên cột" AnchorHorizontal="98%" CtCls="requiredDate" ReadOnly="true"></ext:TextField>
                                    <ext:TextField runat="server" ID="txtUpdateValue" FieldLabel="Giá trị" AnchorHorizontal="98%"></ext:TextField>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
                <Buttons>
                    <ext:Button runat="server" ID="btnUpdate" Text="Cập nhật" Icon="Disk">
                        <Listeners>
                            <Click Handler=" return CheckInput();" />
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="BtnUpdate_Click">
                                <EventMask ShowMask="true" Msg="Đang lưu dữ liệu" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button runat="server" ID="btnClose" Text="Đóng lại" Icon="Decline" Hidden="True">
                        <Listeners>
                            <Click Handler="wdUpdateColumnDynamic.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>
            <ext:Window Modal="true" Hidden="true" runat="server" ID="wdSalaryBoardManage" Constrain="true"
                Title="Chọn bảng lương để cấu hình" Icon="Table" Layout="FormLayout" Width="800" AutoHeight="true">
                <Items>
                    <ext:Container runat="server" ID="Container2" Layout="ColumnLayout" Height="360">
                        <Items>
                            <ext:Hidden ID="hdfSalaryBoardListID" runat="server" />
                            <ext:GridPanel ID="grpSalaryBoardList" runat="server" StripeRows="true" Border="false"
                                AnchorHorizontal="100%" Header="false" Height="350" Title="Danh sách bảng tính lương"
                                AutoExpandColumn="Title">
                                <Store>
                                    <ext:Store ID="storeSalaryBoardList" AutoSave="true" runat="server" GroupField="Year">
                                        <Proxy>
                                            <ext:HttpProxy Method="POST" Url="~/Services/BaseHandler.ashx" />
                                        </Proxy>
                                        <AutoLoadParams>
                                            <ext:Parameter Name="start" Value="={0}" />
                                            <ext:Parameter Name="limit" Value="={15}" />
                                        </AutoLoadParams>
                                        <BaseParams>
                                            <ext:Parameter Name="handlers" Value="SalaryBoardList" />
                                            <ext:Parameter Name="departments" Value="hdfDepartments.getValue()" Mode="Raw" />
                                        </BaseParams>
                                        <Reader>
                                            <ext:JsonReader IDProperty="Id" Root="Data" TotalProperty="TotalRecords">
                                                <Fields>
                                                    <ext:RecordField Name="Id" />
                                                    <ext:RecordField Name="Title" />
                                                    <ext:RecordField Name="Code" />
                                                    <ext:RecordField Name="Description" />
                                                    <ext:RecordField Name="Month" />
                                                    <ext:RecordField Name="Year" />
                                                    <ext:RecordField Name="CreatedDate" />
                                                    <ext:RecordField Name="CreatedBy" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <View>
                                    <ext:GroupingView ID="GroupingView2" runat="server" ForceFit="false" MarkDirty="false"
                                        ShowGroupName="false" EnableNoGroups="true" />
                                </View>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn Header="STT" Width="30" />
                                        <ext:Column ColumnID="Title" Header="Tên bảng lương" Width="160" DataIndex="Title">
                                        </ext:Column>
                                        <ext:Column ColumnID="Title" Header="Mã bảng lương" Width="160" DataIndex="Code">
                                        </ext:Column>
                                        <ext:Column ColumnID="Month" Align="Center" Header="Tháng"
                                            Width="120" DataIndex="Month" />
                                        <ext:Column ColumnID="Year" Align="Center" Header="Năm"
                                            Width="120" DataIndex="Year" Hidden="True" />
                                        <ext:Column ColumnID="CreatedBy" Align="Center" Header="Người tạo" Width="120" DataIndex="CreatedBy" />
                                        <ext:DateColumn Format="dd/MM/yyyy" ColumnID="CreatedDate" Header="Ngày tạo"
                                            Width="80" DataIndex="CreatedDate" />
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModelSalaryBoardList" runat="server">
                                        <Listeners>
                                            <RowSelect Handler="hdfSalaryBoardListID.setValue(RowSelectionModelSalaryBoardList.getSelected().get('Id'));" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" Msg="Đang tải" />
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolbar2" runat="server" PageSize="10">
                                        <Items>
                                            <ext:Label ID="Label2" runat="server" Text="<%$ Resources:HOSO, number_line_per_page%>" />
                                            <ext:ToolbarSpacer ID="ToolbarSpacer7" runat="server" Width="10" />
                                            <ext:ComboBox ID="ComboBox2" runat="server" Width="80">
                                                <Items>
                                                    <ext:ListItem Text="10" />
                                                    <ext:ListItem Text="30" />
                                                    <ext:ListItem Text="50" />
                                                    <ext:ListItem Text="100" />
                                                </Items>
                                                <SelectedItem Value="10" />
                                                <Listeners>
                                                    <Select Handler="#{PagingToolbar2}.pageSize = parseInt(this.getValue()); #{PagingToolbar2}.doLoad();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </Items>
                                        <Listeners>
                                            <Change Handler="RowSelectionModelSalaryBoardList.clearSelections();" />
                                        </Listeners>
                                    </ext:PagingToolbar>
                                </BottomBar>
                            </ext:GridPanel>
                        </Items>
                    </ext:Container>
                </Items>
                <Buttons>
                    <ext:Button ID="btnAcceptSalaryBoardList" runat="server" Icon="Accept" Text="Đồng ý">
                        <Listeners>
                            <Click Handler="if (CheckSelectedRows(grpSalaryBoardList) == false) {return false;}" />
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="ChooseSalaryBoardList_Click">
                                <EventMask ShowMask="true" Msg="Đang chọn bảng lương..." />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button runat="server" ID="btnCloseSalaryBoardList" Text="Đóng lại"
                        Icon="Decline">
                        <Listeners>
                            <Click Handler="wdSalaryBoardManage.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>
            <ext:Window runat="server" Title="Nhập dữ liệu từ file excel" Resizable="true" Layout="FormLayout"
                Padding="6" Width="800" Hidden="true" Icon="UserTick" ID="wdImportExcelFile"
                Modal="true" Constrain="true" Height="450">
                <Items>
                    <ext:Container runat="server" Layout="FormLayout" LabelWidth="150">
                        <Items>
                            <ext:Label runat="server" ID="labelDownload" FieldLabel="Tải tệp tin mẫu">
                            </ext:Label>
                            <ext:Button runat="server" ID="btnDownloadTemplate" Icon="ArrowDown" ToolTip="Tải về" Text="Tải về" Width="100">
                                <DirectEvents>
                                    <Click OnEvent="DownloadTemplate_Click" IsUpload="true" />
                                </DirectEvents>
                                <ToolTips>
                                    <ext:ToolTip ID="ToolTip1" runat="server" Title="Hướng dẫn" Html="Nếu bạn chưa có tệp tin Excel mẫu để nhập liệu. Hãy ấn nút này để tải tệp tin mẫu về máy">
                                    </ext:ToolTip>
                                </ToolTips>
                            </ext:Button>
                            <ext:FileUploadField ID="fileExcel" runat="server" FieldLabel="Tệp tin đính kèm<span style='color:red;'>*</span>"
                                CtCls="requiredData" AnchorHorizontal="98%" Icon="Attach">
                            </ext:FileUploadField>
                            <ext:TextField runat="server" ID="txtSheetName" FieldLabel="Tên sheet Excel" AnchorHorizontal="98%" />
                            <ext:TextField runat="server" ID="txtFromRow" FieldLabel="Từ hàng" AnchorHorizontal="98%" MaskRe="/[0-9.]/" Hidden="true"/>
                            <ext:TextField runat="server" ID="txtToRow" FieldLabel="Đến hàng" AnchorHorizontal="98%" MaskRe="/[0-9.]/" Hidden="true"/>
                        </Items>
                    </ext:Container>
                </Items>
                <Buttons>
                    <ext:Button runat="server" ID="btnUpdateImportExcel" Text="Cập nhật" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnUpdateImportExcel_Click">
                                <EventMask ShowMask="true" Msg="Đang lưu dữ liệu. Vui lòng chờ..." />
                                <ExtraParams>
                                    <ext:Parameter Name="Close" Value="False" />
                                </ExtraParams>
                            </Click>
                        </DirectEvents>
                    </ext:Button>

                    <ext:Button runat="server" ID="btnCloseImport" Text="Đóng lại" Icon="Decline">
                        <Listeners>
                            <Click Handler="wdImportExcelFile.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Hide Handler="ResetWdImportExcelFile();" />
                </Listeners>
            </ext:Window>
        </div>
    </form>
</body>
</html>