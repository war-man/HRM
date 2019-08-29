﻿namespace Web.Core.Framework.SQLAdapter
{
    public class SQLReportRegulationAdapter
    {
        private const string WorkStatusWorking = "%Đang làm việc%";
        private const string WorkStatusLeave = "%Đang nghỉ phép%";
        private const string BusinessTypeRetirement = "NghiHuu";
        private const string BusinessPersonelRotation = "ThuyenChuyenDieuChuyen";

        private const string ReasonRetirement = "%Nghỉ hưu%";
        private const string ReasonTerminate = "%Đơn phương chấm dứt hđlđ/hđlv%";
        private const string ReasonFired = "%Kỷ luật sa thải%";
        private const string ReasonExpiredContract = "%Thỏa thuận chấm dứt%";
        private const string ReasonOther = "%Lý do khác%";
        private const string Dead = "%Từ trần%";

        /// <summary>
        /// Số lượng, chất lượng cán bộ, công chức cấp huyện
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_QuantityDistrictCivilServants(string departments, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            // Lay so luong CCVC
            sql += "IF OBJECT_ID('tempdb..#tmpTongSo') IS NOT NULL DROP Table #tmpTongSo " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpTongSo " +
                   "FROM hr_Record h " +
                   "WHERE h.DepartmentId != 0 " +
                   "GROUP BY h.DepartmentId ";
            // Lay so luong nu trong tung don vi
            sql += "IF OBJECT_ID('tempdb..#tmpNu') IS NOT NULL DROP Table #tmpNu " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpNu " +
                   "FROM hr_Record h " +
                   "WHERE h.Sex='0' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC dang vien
            sql += "IF OBJECT_ID('tempdb..#tmpDangVien') IS NOT NULL DROP Table #tmpDangVien " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpDangVien " +
                   "FROM hr_Record h " +
                   "WHERE h.CPVJoinedDate IS NOT NULL " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC la dan toc thieu so
            sql += "IF OBJECT_ID('tempdb..#tmpDanTocThieuSo') IS NOT NULL DROP Table #tmpDanTocThieuSo " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpDanTocThieuSo " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_Folk dt ON dt.Id = h.FolkId " +
                   "WHERE " +
                   "dt.IsMinority = 1 " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC co ton giao
            sql += "IF OBJECT_ID('tempdb..#tmpTonGiao') IS NOT NULL DROP Table #tmpTonGiao " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpTonGiao " +
                   "FROM hr_Record h " +
                   "WHERE h.ReligionId IS NOT NULL " +
                   "   AND h.ReligionId != 7 " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo chuyen mon (tien si)
            sql += "IF OBJECT_ID('tempdb..#tmpTienSi') IS NOT NULL DROP Table #tmpTienSi " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpTienSi " +
                   "FROM hr_Record h LEFT JOIN cat_Education dt ON dt.Id = h.EducationId " +
                   "WHERE dt.[Group] = 'TS' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo chuyen mon (thac si)
            sql += "IF OBJECT_ID('tempdb..#tmpThacSi') IS NOT NULL DROP Table #tmpThacSi " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpThacSi " +
                   "FROM hr_Record h LEFT JOIN cat_Education dt ON dt.Id = h.EducationId " +
                   "WHERE dt.[Group] = 'ThS' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo chuyen mon (dai hoc)
            sql += "IF OBJECT_ID('tempdb..#tmpDaiHoc') IS NOT NULL DROP Table #tmpDaiHoc " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpDaiHoc " +
                   "FROM hr_Record h LEFT JOIN cat_Education dt ON dt.Id = h.EducationId " +
                   "WHERE dt.[Group] = 'DH' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo chuyen mon (cao dang)
            sql += "IF OBJECT_ID('tempdb..#tmpCaoDang') IS NOT NULL DROP Table #tmpCaoDang " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpCaoDang " +
                   "FROM hr_Record h LEFT JOIN cat_Education dt ON dt.Id = h.EducationId " +
                   "WHERE dt.[Group] = 'CD' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo chuyen mon (trung cap)
            sql += "IF OBJECT_ID('tempdb..#tmpTrungCap') IS NOT NULL DROP Table #tmpTrungCap " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpTrungCap " +
                   "FROM hr_Record h LEFT JOIN cat_Education dt ON dt.Id = h.EducationId " +
                   "WHERE dt.[Group] = 'TC' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo chuyen mon con lai (so cap)
            sql += "IF OBJECT_ID('tempdb..#tmpSoCap') IS NOT NULL DROP Table #tmpSoCap " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpSoCap " +
                   "FROM hr_Record h LEFT JOIN cat_Education dt ON dt.Id = h.EducationId " +
                   "WHERE dt.[Group] = 'SC' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo trinh do cu nhan chinh tri
            sql += "IF OBJECT_ID('tempdb..#tmpCuNhanChinhTri') IS NOT NULL DROP Table #tmpCuNhanChinhTri " +
                   "SELECT h.DepartmentId ,COUNT(h.Id) as 'SoLuong' " +
                   "INTO #tmpCuNhanChinhTri " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_PoliticLevel dtc on dtc.Id = h.PoliticLevelId " +
                   "WHERE dtc.[Group] = 'CN' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo trinh do chinh tri cao cap chinh tri
            sql += "IF OBJECT_ID('tempdb..#tmpCaoCapChinhTri') IS NOT NULL DROP Table #tmpCaoCapChinhTri " +
                   "SELECT h.DepartmentId ,COUNT(h.Id) as 'SoLuong' " +
                   "INTO #tmpCaoCapChinhTri " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_PoliticLevel dtc on dtc.Id = h.PoliticLevelId " +
                   "WHERE dtc.[Group] = 'CC' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo trinh do trung cap
            sql += "IF OBJECT_ID('tempdb..#tmpTrungCapChinhTri') IS NOT NULL DROP Table #tmpTrungCapChinhTri " +
                   "SELECT h.DepartmentId ,COUNT(h.Id) as 'SoLuong' " +
                   "INTO #tmpTrungCapChinhTri " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_PoliticLevel dtc on dtc.Id = h.PoliticLevelId " +
                   "WHERE dtc.[Group] = 'TC' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo trinh do so cap
            sql += "IF OBJECT_ID('tempdb..#tmpSoCapChinhTri') IS NOT NULL DROP Table #tmpSoCapChinhTri " +
                   "SELECT h.DepartmentId ,COUNT(h.Id) as 'SoLuong' " +
                   "INTO #tmpSoCapChinhTri " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_PoliticLevel dtc on dtc.Id = h.PoliticLevelId " +
                   "WHERE dtc.[Group] = 'SC' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC co trinh do trung cap tin hoc tro len
            sql += "IF OBJECT_ID('tempdb..#tmpTrungCapTinHoc') IS NOT NULL DROP Table #tmpTrungCapTinHoc " +
                   "SELECT h.DepartmentId ,COUNT(h.Id) as 'SoLuong' " +
                   "INTO #tmpTrungCapTinHoc " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_ITLevel dth on dth.Id = h.ITLevelId " +
                   "WHERE dth.[Group] = 'TC' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC co chung chi tin hoc
            sql += "IF OBJECT_ID('tempdb..#tmpChungChiTinHoc') IS NOT NULL DROP Table #tmpChungChiTinHoc " +
                   "SELECT h.DepartmentId ,COUNT(h.Id) as 'SoLuong' " +
                   "INTO #tmpChungChiTinHoc " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_ITLevel dth on dth.Id = h.ITLevelId " +
                   "WHERE dth.[Group] = 'CC' " +
                   "GROUP BY h.DepartmentId ";
            //lay so luong CCVC co trinh do tieng Anh dai hoc tro len
            sql += "IF OBJECT_ID('tempdb..#tmpDaiHocTiengAnh') IS NOT NULL DROP Table #tmpDaiHocTiengAnh " +
                   "SELECT DISTINCT tmp.DepartmentId,  " +
                   "COUNT(tmp.LanguageLevelId) AS 'SoLuong' " +
                   "INTO #tmpDaiHocTiengAnh " +
                   "FROM ( " +
                   "SELECT h.Id, h.DepartmentId, h.LanguageLevelId " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_LanguageLevel dnn " +
                   "ON dnn.Id = h.LanguageLevelId " +
                   "UNION " +
                   "SELECT h.Id, h.DepartmentId, hn.LanguageId " +
                   "FROM hr_Record h  " +
                   "LEFT JOIN hr_Language hn " +
                   "on h.Id = hn.RecordId) AS tmp " +
                   "LEFT JOIN cat_LanguageLevel nn " +
                   "ON nn.Id = tmp.LanguageLevelId " +
                   "WHERE tmp.LanguageLevelId != 0 " +
                   "AND nn.[Group] = 'DHTA' " +
                   "GROUP BY tmp.DepartmentId ";

            //lay so luong CCVC co chung chi tieng Anh
            sql += "IF OBJECT_ID('tempdb..#tmpChungChiTiengAnh') IS NOT NULL DROP Table #tmpChungChiTiengAnh " +
                   "	SELECT DISTINCT tmp.DepartmentId,  " +
                   "COUNT(tmp.LanguageLevelId) AS 'SoLuong'	 " +
                   "   INTO #tmpChungChiTiengAnh " +
                   "FROM ( " +
                   "SELECT H.Id, H.DepartmentId, H.LanguageLevelId " +
                   "FROM hr_Record H	 " +
                   "LEFT JOIN cat_LanguageLevel DNN " +
                   "	ON DNN.Id = H.LanguageLevelId " +
                   "UNION " +
                   "SELECT h.Id, h.DepartmentId, hn.LanguageId	 " +
                   "FROM hr_Record h  " +
                   "LEFT JOIN hr_Language hn " +
                   "	on h.Id = hn.RecordId) AS tmp " +
                   "LEFT JOIN cat_LanguageLevel NN " +
                   "	ON NN.Id = tmp.LanguageLevelId " +
                   "WHERE tmp.LanguageLevelId != 0 " +
                   "	AND NN.[Group] = 'CCTA' " +
                   "GROUP BY tmp.DepartmentId ";
            //lay so luong CCVC co trinh do tieng Anh dai hoc ngoai ngu khac
            sql += "IF OBJECT_ID('tempdb..#tmpDaiHocNgoaiNguKhac') IS NOT NULL DROP Table #tmpDaiHocNgoaiNguKhac " +
                   "SELECT DISTINCT tmp.DepartmentId,  " +
                   "COUNT(tmp.LanguageLevelId) AS 'SoLuong'	 " +
                   "INTO #tmpDaiHocNgoaiNguKhac " +
                   "FROM ( " +
                   "SELECT H.Id, H.DepartmentId, H.LanguageLevelId " +
                   "FROM hr_Record H	 " +
                   "LEFT JOIN cat_LanguageLevel DNN " +
                   "ON DNN.Id = H.LanguageLevelId " +
                   "UNION " +
                   "SELECT h.Id, h.DepartmentId, hn.LanguageId	 " +
                   "FROM hr_Record h  " +
                   "LEFT JOIN hr_Language hn " +
                   "on h.Id = hn.RecordId) AS tmp " +
                   "LEFT JOIN cat_LanguageLevel NN " +
                   "ON NN.Id = tmp.LanguageLevelId " +
                   "WHERE tmp.LanguageLevelId != 0 " +
                   "AND NN.[Group] = 'DHNNK' " +
                   "GROUP BY tmp.DepartmentId ";
            //lay so luong CCVC co chung chi ngoai ngu khac
            sql += "IF OBJECT_ID('tempdb..#tmpChungChiNgoaiNguKhac') IS NOT NULL DROP Table #tmpChungChiNgoaiNguKhac " +
                   "SELECT DISTINCT tmp.DepartmentId,  " +
                   "COUNT(tmp.LanguageLevelId) AS 'SoLuong'	 " +
                   "INTO #tmpChungChiNgoaiNguKhac " +
                   "FROM ( " +
                   "SELECT H.Id, H.DepartmentId, H.LanguageLevelId " +
                   "FROM hr_Record H	 " +
                   "LEFT JOIN cat_LanguageLevel DNN " +
                   "ON DNN.Id = H.LanguageLevelId " +
                   "UNION " +
                   "SELECT h.Id, h.DepartmentId, hn.LanguageId	 " +
                   "FROM hr_Record h  " +
                   "LEFT JOIN hr_Language hn " +
                   "on h.Id = hn.RecordId) AS tmp " +
                   "LEFT JOIN cat_LanguageLevel NN " +
                   "ON NN.Id = tmp.LanguageLevelId " +
                   "WHERE tmp.LanguageLevelId != 0 " +
                   "AND NN.[Group] = 'CCNNK' " +
                   "GROUP BY tmp.DepartmentId ";
            //Lay so luong CCVC co chung chi tieng dan toc
            sql += "IF OBJECT_ID('tempdb..#tmpChungChiTiengDanToc') IS NOT NULL DROP Table #tmpChungChiTiengDanToc " +
                   "SELECT DISTINCT tmp.DepartmentId,  " +
                   "COUNT(tmp.LanguageLevelId) AS 'SoLuong' " +
                   "INTO #tmpChungChiTiengDanToc " +
                   "FROM ( " +
                   "SELECT H.Id, H.DepartmentId, H.LanguageLevelId " +
                   "FROM hr_Record H	 " +
                   "LEFT JOIN cat_LanguageLevel DNN " +
                   "ON DNN.Id = H.LanguageLevelId " +
                   "UNION " +
                   "SELECT h.Id, h.DepartmentId, hn.LanguageId	 " +
                   "FROM hr_Record h  " +
                   "LEFT JOIN hr_Language hn " +
                   "on h.Id = hn.RecordId) AS tmp " +
                   "LEFT JOIN cat_LanguageLevel NN " +
                   "ON NN.Id = tmp.LanguageLevelId " +
                   "WHERE tmp.LanguageLevelId != 0 " +
                   "AND NN.[Group] = 'CCTDT' " +
                   "GROUP BY tmp.DepartmentId ";
            //Lay so luong CCVC QLNN Chuyen vien cao cap
            sql += "IF OBJECT_ID('tempdb..#tmpChuyenVienCaoCap') IS NOT NULL DROP Table #tmpChuyenVienCaoCap " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpChuyenVienCaoCap " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_ManagementLevel dtq ON dtq.Id = h.ManagementLevelId " +
                   "WHERE dtq.[Group] = 'CVCC' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC QLNN Chuyen vien chinh
            sql += "IF OBJECT_ID('tempdb..#tmpChuyenVienChinh') IS NOT NULL DROP Table #tmpChuyenVienChinh " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpChuyenVienChinh " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_ManagementLevel dtq ON dtq.Id = h.ManagementLevelId " +
                   "WHERE dtq.[Group] = 'CVC' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC QLNN Chuyen vien
            sql += "IF OBJECT_ID('tempdb..#tmpChuyenVien') IS NOT NULL DROP Table #tmpChuyenVien " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpChuyenVien " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_ManagementLevel dtq ON dtq.Id = h.ManagementLevelId " +
                   "WHERE dtq.[Group] = 'CV' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC QLNN Chua qua dao tao
            sql += "IF OBJECT_ID('tempdb..#tmpChuaQuaDaoTao') IS NOT NULL DROP Table #tmpChuaQuaDaoTao " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpChuaQuaDaoTao " +
                   "FROM hr_Record h " +
                   "LEFT JOIN cat_ManagementLevel dtq ON dtq.Id = h.ManagementLevelId " +
                   "WHERE dtq.[Group] = 'CQDT' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo do tuoi duoi 30
            sql += "IF OBJECT_ID('tempdb..#tmpDuoi30') IS NOT NULL DROP Table #tmpDuoi30 " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpDuoi30 " +
                   "FROM hr_Record h " +
                   "WHERE h.BirthDate IS NOT NULL " +
                   "AND DATEDIFF(YEAR, h.BirthDate, GETDATE())<= 30 " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo do tuoi 31 - 40
            sql += "IF OBJECT_ID('tempdb..#tmp31Den40') IS NOT NULL DROP Table #tmp31Den40 " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmp31Den40 " +
                   "FROM hr_Record h " +
                   "WHERE h.BirthDate IS NOT NULL " +
                   "AND DATEDIFF(YEAR, h.BirthDate, GETDATE())> 30 " +
                   "AND DATEDIFF(YEAR, h.BirthDate, GETDATE())<= 40 " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo do tuoi 41 - 50
            sql += "IF OBJECT_ID('tempdb..#tmp41Den50') IS NOT NULL DROP Table #tmp41Den50 " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmp41Den50 " +
                   "FROM hr_Record h " +
                   "WHERE h.BirthDate IS NOT NULL " +
                   "AND DATEDIFF(YEAR, h.BirthDate, GETDATE())> 40 " +
                   "AND DATEDIFF(YEAR, h.BirthDate, GETDATE())<= 50 " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC tu 51 - 60
            sql += "IF OBJECT_ID('tempdb..#tmp51Den60') IS NOT NULL DROP Table #tmp51Den60 " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmp51Den60 " +
                   "FROM hr_Record h " +
                   "WHERE h.BirthDate IS NOT NULL " +
                   "AND " +
                   "((DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 50 AND" +
                   " DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 55 AND" +
                   " h.Sex = '0') " +
                   "OR " +
                   "(DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 55 " +
                   "AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 60 " +
                   "AND h.Sex = '1')) " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo do tuoi 51 - 55(nu)
            sql += "IF OBJECT_ID('tempdb..#tmpNu51Den55') IS NOT NULL DROP Table #tmpNu51Den55 " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpNu51Den55 " +
                   "FROM hr_Record h " +
                   "WHERE h.BirthDate IS NOT NULL " +
                   "AND DATEDIFF(YEAR, h.BirthDate, GETDATE())> 50 " +
                   "AND DATEDIFF(YEAR, h.BirthDate, GETDATE())<= 55 " +
                   "AND h.Sex = '0' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC theo do tuoi 55 - 60(nam)
            sql += "IF OBJECT_ID('tempdb..#tmpNam56Den60') IS NOT NULL DROP Table #tmpNam56Den60 " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpNam56Den60 " +
                   "FROM hr_Record h " +
                   "WHERE h.BirthDate IS NOT NULL " +
                   "AND DATEDIFF(YEAR, h.BirthDate, GETDATE())> 55 " +
                   "AND DATEDIFF(YEAR, h.BirthDate, GETDATE())<= 60 " +
                   "AND h.Sex = '1' " +
                   "GROUP BY h.DepartmentId ";
            //Lay so luong CCVC tren tuoi nghi huu
            sql += "IF OBJECT_ID('tempdb..#tmpTrenTuoiNghiHuu') IS NOT NULL DROP Table #tmpTrenTuoiNghiHuu " +
                   "SELECT h.DepartmentId, COUNT(h.Id) AS 'SoLuong' " +
                   "INTO #tmpTrenTuoiNghiHuu " +
                   "FROM hr_Record h " +
                   " WHERE h.BirthDate IS NOT NULL " +
                   "AND " +
                   "((DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 55 AND h.Sex = '0') " +
                   "OR " +
                   "(DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 60 AND h.Sex = '1')) " +
                   "GROUP BY h.DepartmentId ";
            //Ngach CC
            //Chuyen vien cao cap & TD
            sql += "IF OBJECT_ID('tempdb..#tempNgach1') IS NOT NULL DROP Table #tempNgach1 " +
                   "SELECT h.DepartmentId, COUNT(*) AS 'SoLuong' " +
                   "INTO #tempNgach1 " +
                   "FROM hr_Record AS h " +
                   "   LEFT JOIN (SELECT RecordId, QuantumId FROM sal_SalaryDecision GROUP BY RecordId, QuantumId) s" +
                   "       ON h.Id = s.RecordId" +
                   " WHERE s.QuantumId IN (SELECT dn.Id " +
                   "FROM cat_Quantum dn," +
                   "cat_GroupQuantum nn " +
                   " WHERE dn.GroupQuantumId = nn.Id " +
                   "AND nn.[Group] = 'CVCC') " +
                   "GROUP BY h.DepartmentId ";

            //Chuyen vien chinh & TD
            sql += "IF OBJECT_ID('tempdb..#tempNgach2') IS NOT NULL DROP Table #tempNgach2 " +
                   "SELECT h.DepartmentId, COUNT(*) AS 'SoLuong' " +
                   "INTO #tempNgach2 " +
                   "FROM hr_Record AS h " +
                   "   LEFT JOIN (SELECT RecordId, QuantumId FROM sal_SalaryDecision GROUP BY RecordId, QuantumId) s" +
                   "       ON h.Id = s.RecordId" +
                   " WHERE s.QuantumId IN (SELECT dn.Id " +
                   "FROM cat_Quantum dn," +
                   "cat_GroupQuantum nn " +
                   "WHERE dn.GroupQuantumId = nn.Id " +
                   "AND nn.[Group] = 'CVC') " +
                   "GROUP BY h.DepartmentId ";

            //Chuyen vien & TD
            sql += "IF OBJECT_ID('tempdb..#tempNgach3') IS NOT NULL DROP Table #tempNgach3 " +
                   "SELECT h.DepartmentId, COUNT(*) AS 'SoLuong' " +
                   "INTO #tempNgach3 " +
                   "FROM hr_Record AS h " +
                   "   LEFT JOIN (SELECT RecordId, QuantumId FROM sal_SalaryDecision GROUP BY RecordId, QuantumId) s" +
                   "       ON h.Id = s.RecordId" +
                   " WHERE s.QuantumId IN (SELECT dn.Id " +
                   "FROM cat_Quantum dn," +
                   "cat_GroupQuantum nn " +
                   "WHERE dn.GroupQuantumId = nn.Id " +
                   "AND nn.[Group] = 'CV') " +
                   "GROUP BY h.DepartmentId ";

            //Can su & TD
            sql += "IF OBJECT_ID('tempdb..#tempNgach4') IS NOT NULL DROP Table #tempNgach4 " +
                   "SELECT h.DepartmentId, COUNT(*) AS 'SoLuong' " +
                   "INTO #tempNgach4 " +
                   "FROM hr_Record AS h " +
                   "   LEFT JOIN (SELECT RecordId, QuantumId FROM sal_SalaryDecision GROUP BY RecordId, QuantumId) s" +
                   "       ON h.Id = s.RecordId" +
                   " WHERE s.QuantumId IN (SELECT dn.Id " +
                   "FROM cat_Quantum dn," +
                   "cat_GroupQuantum nn " +
                   "WHERE dn.GroupQuantumId = nn.Id " +
                   "AND nn.[Group] = 'CS') " +
                   "GROUP BY h.DepartmentId ";

            //Nhan vien
            sql += "IF OBJECT_ID('tempdb..#tempNgach5') IS NOT NULL DROP Table #tempNgach5 " +
                   "SELECT h.DepartmentId, COUNT(*) AS 'SoLuong' " +
                   "INTO #tempNgach5 " +
                   "FROM hr_Record AS h " +
                   "   LEFT JOIN (SELECT RecordId, QuantumId FROM sal_SalaryDecision GROUP BY RecordId, QuantumId) s" +
                   "       ON h.Id = s.RecordId" +
                   " WHERE s.QuantumId IN (SELECT dn.Id " +
                   "FROM cat_Quantum dn," +
                   "cat_GroupQuantum nn " +
                   "WHERE dn.GroupQuantumId = nn.Id " +
                   "AND nn.[Group] = 'NV') " +
                   "GROUP BY h.DepartmentId ";
            // Lay danh sach co quan to chuc truc thuoc
            sql += "IF OBJECT_ID('tempdb..#tmpA') IS NOT NULL DROP Table #tmpA " +
                   "SELECT dd.Id AS 'DepartmentId', dd.Name " +
                   "INTO #tmpA " +
                   "FROM cat_Department dd ";
            // Xoa bang #tmpB neu da ton tai
            sql += "IF OBJECT_ID('tempdb..#tmpB') IS NOT NULL DROP Table #tmpB ";
            // Tong hop
            sql += "SELECT " +
                   "'' AS 'STT', " +
                   "#tmpA.DepartmentId, " +
                   "#tmpA.Name, " +
                   // Tong so
                   "#tmpTongSo.SoLuong AS 'TongSo', " +
                   // Trong do
                   "#tmpNu.SoLuong AS 'Nu', " +
                   "#tmpDangVien.SoLuong AS 'DangVien', " +
                   "#tmpDanTocThieuSo.SoLuong AS 'DanTocThieuSo', " +
                   "#tmpTonGiao.SoLuong AS 'TonGiao', " +
                   // Trinh do chuyen mon
                   "#tmpTienSi.SoLuong AS 'TienSi', " +
                   "#tmpThacSi.SoLuong AS 'ThacSi', " +
                   "#tmpDaiHoc.SoLuong AS 'DaiHoc', " +
                   "#tmpCaoDang.SoLuong AS 'CaoDang', " +
                   "#tmpTrungCap.SoLuong AS 'TrungCap', " +
                   "#tmpSoCap.SoLuong AS 'SoCap', " +
                   // Trinh do chinh tri
                   "#tmpCuNhanChinhTri.SoLuong as 'CuNhanChinhTri', " +
                   "#tmpCaoCapChinhTri.SoLuong as 'CaoCapChinhTri', " +
                   "#tmpTrungCapChinhTri.SoLuong as 'TrungCapChinhTri', " +
                   "#tmpSoCapChinhTri.SoLuong as 'SoCapChinhTri'," +
                   // Trinh do tin hoc
                   "#tmpTrungCapTinHoc.SoLuong as 'TrungCapTinHoc', " +
                   "#tmpChungChiTinHoc.SoLuong as 'ChungChiTinHoc', " +
                   // Trinh do tieng Anh
                   "#tmpDaiHocTiengAnh.SoLuong as 'DaiHocTiengAnh', " +
                   "#tmpChungChiTiengAnh.SoLuong as 'ChungChiTiengAnh', " +
                   // Trinh do ngoai ngu khac
                   "#tmpDaiHocNgoaiNguKhac.SoLuong as 'DaiHocNgoaiNguKhac', " +
                   "#tmpChungChiNgoaiNguKhac.SoLuong as 'ChungChiNgoaiNguKhac', " +
                   //Chung chi tieng dan toc
                   "#tmpChungChiTiengDanToc.SoLuong as 'ChungChiTiengDanToc', " +
                   // Cap bac QLNN
                   "#tmpChuyenVienCaoCap.SoLuong AS 'ChuyenVienCaoCap', " +
                   "#tmpChuyenVienChinh.SoLuong AS 'ChuyenVienChinh', " +
                   "#tmpChuyenVien.SoLuong AS 'ChuyenVien', " +
                   "#tmpChuaQuaDaoTao.SoLuong AS 'ChuaQuaDaoTao', " +
                   // Do tuoi
                   "#tmpDuoi30.SoLuong AS 'Duoi30'," +
                   "#tmp31Den40.SoLuong AS 'Tu31Den40', " +
                   "#tmp41Den50.SoLuong AS 'Tu41Den50', " +
                   "#tmp51Den60.SoLuong AS 'Tu51Den60', " +
                   "#tmpNu51Den55.SoLuong AS 'Nu51Den55', " +
                   "#tmpNam56Den60.SoLuong AS 'Nam56Den60', " +
                   "#tmpTrenTuoiNghiHuu.SoLuong AS 'TrenTuoiNghiHuu', " +
                   // Ngach CCVC
                   "#tempNgach1.SoLuong AS 'NgachCVCC', " +
                   "#tempNgach2.SoLuong AS 'NgachCVC', " +
                   "#tempNgach3.SoLuong AS 'NgachCV', " +
                   "#tempNgach4.SoLuong AS 'NgachCS', " +
                   "#tempNgach5.SoLuong AS 'NgachNV', " +
                   // Tong hop
                   "0 AS 'xTongSo', " +
                   // Trong do
                   "0 AS 'xNu', " +
                   "0 AS 'xDangVien', " +
                   "0 AS 'xDanTocThieuSo', " +
                   "0 AS 'xTonGiao', " +
                   // Trinh do chuyen mon
                   "0 AS 'xTienSi', " +
                   "0 AS 'xThacSi', " +
                   "0 AS 'xDaiHoc', " +
                   "0 AS 'xCaoDang'," +
                   "0 AS 'xTrungCap', " +
                   "0 AS 'xSoCap', " +
                   // Trinh do chinh tri
                   "0 as 'xCuNhanChinhTri', " +
                   "0 as 'xCaoCapChinhTri', " +
                   "0 as 'xTrungCapChinhTri', " +
                   "0 as 'xSoCapChinhTri', " +
                   // Trinh do tin hoc
                   "0 as 'xTrungCapTinHoc', " +
                   "0 as 'xChungChiTinHoc', " +
                   // Trinh do tieng Anh
                   "0 as 'xDaiHocTiengAnh', " +
                   "0 as 'xChungChiTiengAnh', " +
                   // Trinh do ngoai ngu khac
                   "0 as 'xDaiHocNgoaiNguKhac', " +
                   "0 as 'xChungChiNgoaiNguKhac', " +
                   //Chung chi tieng dan toc
                   "0 as 'xChungChiTiengDanToc', " +
                   // Cap bac QLNN
                   "0 AS 'xChuyenVienCaoCap', " +
                   "0 AS 'xChuyenVienChinh', " +
                   "0 AS 'xChuyenVien', " +
                   "0 AS 'xChuaQuaDaoTao', " +
                   // Do tuoi
                   "0 AS 'xDuoi30', " +
                   "0 AS 'x31Den40', " +
                   "0 AS 'x41Den50', " +
                   "0 AS 'x51Den60', " +
                   "0 AS 'xNu51Den55', " +
                   "0 AS 'xNam56Den60', " +
                   "0 AS 'xTrenTuoiNghiHuu', " +
                   // Ngach CCVC
                   "0 AS 'xNgachCVCC', " +
                   "0 AS 'xNgachCVC', " +
                   "0 AS 'xNgachCV', " +
                   "0 AS 'xNgachCS', " +
                   "0 AS 'xNgachNV' " +
                   // Day du lieu vao bang #tmpB
                   "INTO #tmpB " +
                   //
                   // join table
                   //
                   "FROM #tmpA " +
                   // Tong so
                   "LEFT JOIN #tmpTongSo ON #tmpTongSo.DepartmentId=#tmpA.DepartmentId " +
                   // Trong do
                   "LEFT JOIN #tmpNu ON #tmpNu.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpDangVien ON #tmpDangVien.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpDanTocThieuSo ON #tmpDanTocThieuSo.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpTonGiao ON #tmpTonGiao.DepartmentId=#tmpA.DepartmentId " +
                   // Trinh do chuyen mon
                   "LEFT JOIN #tmpTienSi ON #tmpTienSi.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpThacSi ON #tmpThacSi.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpDaiHoc ON #tmpDaiHoc.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpCaoDang ON #tmpCaoDang.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpTrungCap ON #tmpTrungCap.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpSoCap ON #tmpSoCap.DepartmentId=#tmpA.DepartmentId " +
                   // Trinh do chinh tri
                   "LEFT JOIN #tmpCuNhanChinhTri ON #tmpCuNhanChinhTri.DepartmentId= #tmpA.DepartmentId " +
                   "LEFT JOIN #tmpCaoCapChinhTri ON #tmpCaoCapChinhTri.DepartmentId= #tmpA.DepartmentId " +
                   "LEFT JOIN #tmpTrungCapChinhTri ON #tmpTrungCapChinhTri.DepartmentId= #tmpA.DepartmentId " +
                   "LEFT JOIN #tmpSoCapChinhTri ON #tmpSoCapChinhTri.DepartmentId= #tmpA.DepartmentId " +
                   "LEFT JOIN #tmpTrungCapTinHoc ON #tmpTrungCapTinHoc.DepartmentId= #tmpA.DepartmentId " +
                   "LEFT JOIN #tmpChungChiTinHoc ON #tmpChungChiTinHoc.DepartmentId= #tmpA.DepartmentId " +
                   // Trinh do tieng anh
                   "LEFT JOIN #tmpDaiHocTiengAnh ON #tmpDaiHocTiengAnh.DepartmentId= #tmpA.DepartmentId " +
                   "LEFT JOIN #tmpChungChiTiengAnh ON #tmpChungChiTiengAnh.DepartmentId= #tmpA.DepartmentId " +
                   // Trinh do ngoai ngu khac
                   "LEFT JOIN #tmpDaiHocNgoaiNguKhac ON #tmpDaiHocNgoaiNguKhac.DepartmentId= #tmpA.DepartmentId " +
                   "LEFT JOIN #tmpChungChiNgoaiNguKhac ON #tmpChungChiNgoaiNguKhac.DepartmentId= #tmpA.DepartmentId " +
                   //Chung chi tieng dan toc
                   "LEFT JOIN #tmpChungChiTiengDanToc ON #tmpChungChiTiengDanToc.DepartmentId = #tmpA.DepartmentId " +
                   // Cap bac QLNN
                   "LEFT JOIN #tmpChuyenVienCaoCap ON #tmpChuyenVienCaoCap.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpChuyenVienChinh ON #tmpChuyenVienChinh.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpChuyenVien ON #tmpChuyenVien.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpChuaQuaDaoTao ON #tmpChuaQuaDaoTao.DepartmentId=#tmpA.DepartmentId " +
                   // Do tuoi
                   "LEFT JOIN #tmpDuoi30 ON #tmpDuoi30.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmp31Den40 ON #tmp31Den40.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmp41Den50 ON #tmp41Den50.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmp51Den60 ON #tmp51Den60.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpNu51Den55 ON #tmpNu51Den55.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpNam56Den60 ON #tmpNam56Den60.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tmpTrenTuoiNghiHuu ON #tmpTrenTuoiNghiHuu.DepartmentId=#tmpA.DepartmentId " +
                   // Ngach CCVC
                   "LEFT JOIN #tempNgach1 ON #tempNgach1.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tempNgach2 ON #tempNgach2.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tempNgach3 ON #tempNgach3.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tempNgach4 ON #tempNgach4.DepartmentId=#tmpA.DepartmentId " +
                   "LEFT JOIN #tempNgach5 ON #tempNgach5.DepartmentId=#tmpA.DepartmentId " +
                   //
                   // where condition
                   //
                   "WHERE #tmpA.DepartmentId IN (SELECT [Id] FROM [cat_Department] WHERE [Id] IN ({0}) AND [IsLocked]='0') "
                       .FormatWith(departments);
            if(!string.IsNullOrEmpty(condition))
            {
                sql += " AND {0}".FormatWith(condition);
            }

            // Tinh tong
            sql += "UPDATE	#tmpB " +
                   "SET " +
                   // Tong so
                   "xTongSo = xwTongSo, " +
                   // Trong do
                   "xNu = xwNu, " +
                   "xDangVien = xwDangVien, " +
                   "xDanTocThieuSo = xwDanTocThieuSo, " +
                   "xTonGiao = xwTonGiao, " +
                   // Tring do chuyen mon
                   "xTienSi = xwTienSi, " +
                   "xThacSi = xwThacSi, " +
                   "xDaiHoc = xwDaiHoc, " +
                   "xCaoDang = xwCaoDang, " +
                   "xTrungCap = xwTrungCap, " +
                   "xSoCap = xwSoCap, " +
                   // Trinh do chinh tri
                   "xCuNhanChinhTri = xwCuNhanChinhTri, " +
                   "xCaoCapChinhTri = xwCaoCapChinhTri, " +
                   "xTrungCapChinhTri = xwTrungCapChinhTri, " +
                   "xSoCapChinhTri = xwSocapChinhTri, " +
                   // Trinh do tin hoc
                   "xTrungCapTinHoc = xwTrungCapTinHoc, " +
                   "xChungChiTinHoc = xwChungChiTinHoc, " +
                   // Trinh do tieng anh
                   "xDaiHocTiengAnh = xwDaiHocTiengAnh, " +
                   "xChungChiTiengAnh = xwChungChiTiengAnh, " +
                   // Trinh do ngoai ngu khac
                   "xDaiHocNgoaiNguKhac = xwDaiHocNgoaiNguKhac, " +
                   "xChungChiNgoaiNguKhac = xwChungChiNgoaiNguKhac, " +
                   //Chung chi tieng dan toc
                   "xChungChiTiengDanToc = xwChungChiTiengDanToc, " +
                   // Trinh do QLNN
                   "xChuyenVienCaoCap = xwChuyenVienCaoCap, " +
                   "xChuyenVienChinh = xwChuyenVienChinh, " +
                   "xChuyenVien = xwChuyenVien, " +
                   "xChuaQuaDaoTao = xwChuaQuaDaoTao, " +
                   // Do tuoi
                   "xDuoi30 = xwDuoi30, " +
                   "x31Den40 = xw31Den40, " +
                   "x41Den50 = xw41Den50, " +
                   "x51Den60 = xw51Den60, " +
                   "xNu51Den55 = xwNu51Den55, " +
                   "xNam56Den60 = xwNam56Den60, " +
                   "xTrenTuoiNghiHuu = xwTrenTuoiNghiHuu, " +
                   // Ngach CCVC
                   "xNgachCVCC = xwNgachCVCC, " +
                   "xNgachCVC = xwNgachCVC, " +
                   "xNgachCV = xwNgachCV, " +
                   "xNgachCS = xwNgachCS, " +
                   "xNgachNV = xwNgachNV " +
                   //
                   // From table
                   //
                   "FROM " +
                   "#tmpB " +
                   //
                   // Join table
                   "INNER JOIN " +
                   "(SELECT " +
                   // Tong so
                   "SUM(CASE WHEN #tmpB.TongSo IS NULL THEN 0 ELSE #tmpB.TongSo END) AS xwTongSo, " +
                   // Trong do
                   "SUM(CASE WHEN #tmpB.Nu IS NULL THEN 0 ELSE #tmpB.Nu END) AS xwNu, " +
                   "SUM(CASE WHEN #tmpB.DangVien IS NULL THEN 0 ELSE #tmpB.DangVien END) AS xwDangVien, " +
                   "SUM(CASE WHEN #tmpB.DanTocThieuSo IS NULL THEN 0 ELSE #tmpB.DanTocThieuSo  END) AS xwDanTocThieuSo, " +
                   "SUM(CASE WHEN #tmpB.TonGiao IS NULL THEN 0 ELSE #tmpB.TonGiao END) AS xwTongiao, " +
                   // Trinh do chuyen mon
                   "SUM(CASE WHEN #tmpB.TienSi IS NULL THEN 0 ELSE #tmpB.TienSi END) AS xwTienSi, " +
                   "SUM(CASE WHEN #tmpB.ThacSi IS NULL THEN 0 ELSE #tmpB.ThacSi END) AS xwThacSi, " +
                   "SUM(CASE WHEN #tmpB.DaiHoc IS NULL THEN 0 ELSE #tmpB.DaiHoc END) AS xwDaiHoc, " +
                   "SUM(CASE WHEN #tmpB.CaoDang IS NULL THEN 0 ELSE #tmpB.CaoDang END) AS xwCaoDang, " +
                   "SUM(CASE WHEN #tmpB.TrungCap IS NULL THEN 0 ELSE #tmpB.TrungCap END ) AS xwTrungCap, " +
                   "SUM(CASE WHEN #tmpB.SoCap IS NULL THEN 0 ELSE  #tmpB.SoCap END) AS xwSoCap, " +
                   // Trinh do chinh tri
                   "SUM(CASE WHEN #tmpB.CuNhanChinhTri IS NULL THEN 0 ELSE #tmpB.CuNhanChinhTri END) AS xwCuNhanChinhTri, " +
                   "SUM(CASE WHEN #tmpB.CaoCapChinhTri IS NULL THEN 0 ELSE #tmpB.CaoCapChinhTri END) AS xwCaoCapChinhTri, " +
                   "SUM(CASE WHEN #tmpB.TrungCapChinhTri IS NULL THEN 0 ELSE #tmpB.TrungCapChinhTri END) AS xwTrungCapChinhTri, " +
                   "SUM(CASE WHEN #tmpB.SoCapChinhTri IS NULL THEN 0 ELSE #tmpB.SoCapChinhTri END) AS xwSoCapChinhTri, " +
                   // Trinh do tin hoc
                   "SUM(CASE WHEN #tmpB.TrungCapTinHoc IS NULL THEN 0 ELSE #tmpB.TrungCapTinHoc END) AS xwTrungCapTinHoc, " +
                   "SUM(CASE WHEN #tmpB.ChungChiTinHoc IS NULL THEN 0 ELSE #tmpB.ChungChiTinHoc END )AS xwChungChiTinHoc, " +
                   // Trinh do tieng anh
                   "SUM(CASE WHEN #tmpB.DaiHocTiengAnh IS NULL THEN 0 ELSE #tmpB.DaiHocTiengAnh END) AS xwDaiHocTiengAnh, " +
                   "SUM(CASE WHEN #tmpB.ChungChiTiengAnh IS NULL THEN 0 ELSE #tmpB.ChungChiTiengAnh END ) AS xwChungChiTiengAnh, " +
                   // Trinh do ngoai ngu khac
                   "SUM(CASE WHEN #tmpB.DaiHocNgoaiNguKhac IS NULL THEN 0 ELSE #tmpB.DaiHocNgoaiNguKhac END) AS xwDaiHocNgoaiNguKhac, " +
                   "SUM(CASE WHEN #tmpB.ChungChiNgoaiNguKhac IS NULL THEN 0 ELSE #tmpB.ChungChiNgoaiNguKhac END) AS xwChungChiNgoaiNguKhac, " +
                   //Chung chi tieng dan toc
                   "SUM(CASE WHEN #tmpB.ChungChiTiengDanToc IS NULL THEN 0 ELSE #tmpB.ChungChiTiengDanToc END) AS xwChungChiTiengDanToc, " +
                   // Trinh do QLNN
                   "SUM(CASE WHEN #tmpB.ChuyenVienCaoCap IS NULL THEN 0 ELSE #tmpB.ChuyenVienCaoCap END) AS xwChuyenVienCaoCap, " +
                   "SUM(CASE WHEN #tmpB.ChuyenVienChinh IS NULL THEN 0 ELSE #tmpB.ChuyenVienChinh  END) AS  xwChuyenVienChinh, " +
                   "SUM(CASE WHEN #tmpB.ChuyenVien IS NULL THEN 0 ELSE #tmpB.ChuyenVien  END) AS xwChuyenVien, " +
                   "SUM(CASE WHEN #tmpB.ChuaQuaDaoTao IS NULL THEN 0 ELSE #tmpB.ChuaQuaDaoTao END) AS xwChuaQuaDaoTao, " +
                   // Do tuoi
                   "SUM(CASE WHEN #tmpB.Duoi30 IS NULL THEN 0 ELSE #tmpB.Duoi30 END) AS xwDuoi30, " +
                   "SUM(CASE WHEN #tmpB.Tu31Den40 IS NULL THEN 0 ELSE #tmpB.Tu31Den40 END) AS xw31Den40, " +
                   "SUM(CASE WHEN #tmpB.Tu41Den50 IS NULL THEN 0 ELSE #tmpB.Tu41Den50 END) AS xw41Den50, " +
                   "SUM(CASE WHEN #tmpB.Tu51Den60 IS NULL THEN 0 ELSE #tmpB.Tu51Den60 END) AS xw51Den60, " +
                   "SUM(CASE WHEN #tmpB.Nu51Den55 IS NULL THEN 0 ELSE #tmpB.Nu51Den55 END) AS xwNu51Den55, " +
                   "SUM(CASE WHEN #tmpB.Nam56Den60 IS NULL THEN 0 ELSE #tmpB.Nam56Den60 END) AS xwNam56Den60, " +
                   "SUM(CASE WHEN #tmpB.TrenTuoiNghiHuu IS NULL THEN 0 ELSE #tmpB.TrenTuoiNghiHuu END) AS xwTrenTuoiNghiHuu, " +
                   // Ngach CCVC
                   "SUM(CASE WHEN #tmpB.NgachCVCC IS NULL THEN 0 ELSE #tmpB.NgachCVCC  END) AS xwNgachCVCC, " +
                   "SUM(CASE WHEN #tmpB.NgachCVC IS NULL THEN 0 ELSE  #tmpB.NgachCVC END) AS xwNgachCVC, " +
                   "SUM(CASE WHEN #tmpB.NgachCV IS NULL THEN 0 ELSE #tmpB.NgachCV END) AS xwNgachCV, " +
                   "SUM(CASE WHEN #tmpB.NgachCS IS NULL THEN 0 ELSE #tmpB.NgachCS  END) AS xwNgachCS, " +
                   "SUM(CASE WHEN #tmpB.NgachNV IS NULL THEN 0 ELSE #tmpB.NgachNV END) AS xwNgachNV " +
                   //
                   // From table
                   //
                   "FROM #tmpB) AS TMP " +
                   "ON 1=1 ";
            // select all from #tmpB
            sql += "SELECT * FROM #tmpB ";

            // return
            return sql;
        }

        /// <summary>
        /// Số lượng, chất lượng cán bộ, công chức cấp xã
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_QuantityCommuneCivilServants(string departments, string fromDate, string toDate, string condition)
        {
            // init sql
            var sql = string.Empty;
            //
            // create sql
            //
            // xoa bang #tmpA neu da ton tai
            sql += "IF OBJECT_ID('tempdb..#tmpB') IS NOT NULL DROP Table #tmpB ";
            // lay du lieu vao bang #tmpB
            sql += "SELECT " +
                   "FullName AS HoTen, " +
                   "(SELECT dp.Name FROM cat_Department dp WHERE rc.DepartmentId = dp.Id ) AS 'TenDonVi', " +
                   // Trong do
                   "CASE WHEN Sex = '0' THEN 'X' ELSE '' END AS Nu, " +
                   "CASE WHEN (rc.CPVOfficialJoinedDate IS NOT NULL) THEN 'X' ELSE '' END AS DangVien, " +
                   "CASE WHEN (SELECT TOP 1 IsMinority FROM cat_Folk WHERE rc.FolkId = Id) = '1' THEN 'X' ELSE '' END AS DanTocThieuSo, " +
                   "CASE WHEN ReligionId IS NOT NULL AND ReligionId != 0 THEN 'X' ELSE '' END AS TonGiao, " +
                   // Chuc danh
                   "CASE WHEN (SELECT TOP 1 Name FROM cat_Position cp WHERE rc.PositionId = cp.Id ) IS NOT NULL THEN 'X' ELSE '' END AS CanBoCapXa, " +
                   "CASE WHEN (SELECT TOP 1 Name FROM cat_JobTitle cjt WHERE rc.JobTitleId = cjt.Id ) IS NOT NULL THEN 'X' ELSE '' END AS CongChucChuyenMon, " +
                   // Trinh do chuyen mon
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education ce  WHERE rc.EducationId = ce.Id) = 'TS' THEN 'X' ELSE '' END AS TienSi, " +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education ce  WHERE rc.EducationId = ce.Id) = 'ThS' THEN 'X' ELSE '' END AS ThacSi, " +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education ce  WHERE rc.EducationId = ce.Id) = 'DH' THEN 'X' ELSE '' END AS DaiHoc, " +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education ce  WHERE rc.EducationId = ce.Id) = 'CD' THEN 'X' ELSE '' END AS CaoDang, " +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education ce  WHERE rc.EducationId = ce.Id) = 'TC' THEN 'X' ELSE '' END AS TrungCap, " +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education ce  WHERE rc.EducationId = ce.Id) = 'SC' THEN 'X' ELSE '' END AS SoCap, " +
                   // Trinh do van hoa
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_BasicEducation WHERE rc.BasicEducationId = Id) = 'THPT' THEN 'X' ELSE '' END AS THPT, " +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_BasicEducation WHERE rc.BasicEducationId = Id) = 'THCS' THEN 'X' ELSE '' END AS THCS, " +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_BasicEducation WHERE rc.BasicEducationId = Id) = 'TieuHoc' THEN 'X' ELSE '' END AS TieuHoc, " +
                   // Trinh do chinh tri
                   "CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel cpl WHERE rc.PoliticLevelId = cpl.Id) = '4' THEN 'X' ELSE '' END AS CuNhanChinhTri, " +
                   "CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel cpl WHERE rc.PoliticLevelId = cpl.Id) = '3' THEN 'X' ELSE '' END AS CaoCapChinhTri, " +
                   "CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel cpl WHERE rc.PoliticLevelId = cpl.Id) = '2' THEN 'X' ELSE '' END AS TrungCapChinhTri, " +
                   "CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel cpl WHERE rc.PoliticLevelId = cpl.Id) = '1' THEN 'X' ELSE '' END AS SoCapChinhTri, " +
                   // Trinh do tin hoc
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel cil WHERE cil.Id = rc.ITLevelId) = 'TC' THEN 'X' ELSE '' END AS TrungCapTinHoc, " +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel cil WHERE cil.Id = rc.ITLevelId) = 'CC' THEN 'X' ELSE '' END AS ChungChiTinHoc, " +
                   // Trinh do tieng anh
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel cll WHERE rc.LanguageLevelId = cll.Id) = 'DHTA' OR (SELECT cll.[Group] FROM cat_LanguageLevel cll LEFT JOIN hr_Language hl ON hl.RecordId = rc.Id  WHERE cll.Id = hl.LanguageId AND cll.[Group] = 'DHTA') = 'DHTA' THEN 'X' ELSE '' END AS DaiHocTiengAnh," +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel cll WHERE rc.LanguageLevelId = cll.Id) = 'CCTA' OR (SELECT cll.[Group] FROM cat_LanguageLevel cll LEFT JOIN hr_Language hl ON hl.RecordId = rc.Id  WHERE cll.Id = hl.LanguageId AND cll.[Group] = 'CCTA') = 'CCTA' THEN 'X' ELSE '' END AS ChungChiTiengAnh," +
                   // Trinh do ngoai ngu khac
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel cll WHERE rc.LanguageLevelId = cll.Id) = 'DHNNK' OR (SELECT cll.[Group] FROM cat_LanguageLevel cll LEFT JOIN hr_Language hl ON hl.RecordId = rc.Id  WHERE cll.Id = hl.LanguageId AND cll.[Group] = 'DHNNK') = 'DHNNK' THEN 'X' ELSE '' END AS DaiHocNgoaiNguKhac," +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel cll WHERE rc.LanguageLevelId = cll.Id) = 'CCNNK' OR (SELECT cll.[Group] FROM cat_LanguageLevel cll LEFT JOIN hr_Language hl ON hl.RecordId = rc.Id  WHERE cll.Id = hl.LanguageId AND cll.[Group] = 'CCNNK') = 'CCNNK' THEN 'X' ELSE '' END AS ChungChiNgoaiNguKhac, " +
                   // Trinh do tieng dan toc
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel cll WHERE rc.LanguageLevelId = cll.Id) = 'CCTDT' OR (SELECT cll.[Group] FROM cat_LanguageLevel cll LEFT JOIN hr_Language hl ON hl.RecordId = rc.Id  WHERE cll.Id = hl.LanguageId AND cll.[Group] = 'CCTDT') = 'CCTDT' THEN 'X' ELSE '' END AS ChungChiTiengDanToc," +
                   // Trinh do QLNN
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel cml WHERE rc.ManagementLevelId = cml.Id) = 'CVCC' THEN 'X' ELSE '' END AS ChuyenVienCaoCap, " +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel cml WHERE rc.ManagementLevelId = cml.Id) = 'CVC' THEN 'X' ELSE '' END AS ChuyenVienChinh, " +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel cml WHERE rc.ManagementLevelId = cml.Id) = 'CV' THEN 'X' ELSE '' END AS ChuyenVien, " +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel cml WHERE rc.ManagementLevelId = cml.Id) = 'CQDT' THEN 'X' ELSE '' END AS ChuaQuaDaoTao, " +
                   // Do tuoi
                   "CASE WHEN (rc.BirthDate IS NOT NULL AND DATEDIFF(YEAR, rc.BirthDate, GETDATE()) <= 30) THEN 'X' ELSE '' END AS Duoi30, " +
                   "CASE WHEN (rc.BirthDate IS NOT NULL AND DATEDIFF(YEAR, rc.BirthDate, GETDATE()) > 30 AND DATEDIFF(YEAR, rc.BirthDate, GETDATE()) <= 40) THEN 'X' ELSE '' END AS Tu31Den40, " +
                   "CASE WHEN (rc.BirthDate IS NOT NULL AND DATEDIFF(YEAR, rc.BirthDate, GETDATE()) > 40 AND DATEDIFF(YEAR, rc.BirthDate, GETDATE()) <= 50) THEN 'X' ELSE '' END AS Tu41Den50, " +
                   "CASE WHEN (rc.BirthDate IS NOT NULL AND DATEDIFF(YEAR, rc.BirthDate, GETDATE()) > 50 AND DATEDIFF(YEAR, rc.BirthDate, GETDATE()) <= 60) THEN 'X' ELSE '' END AS Tu51Den60, " +
                   "CASE WHEN (rc.BirthDate IS NOT NULL AND DATEDIFF(YEAR, rc.BirthDate, GETDATE()) > 50 AND DATEDIFF(YEAR, rc.BirthDate, GETDATE()) <= 55 AND rc.Sex = '0') THEN 'X' ELSE '' END AS Nu51Den55, " +
                   "CASE WHEN (rc.BirthDate IS NOT NULL AND DATEDIFF(YEAR, rc.BirthDate, GETDATE()) > 55 AND DATEDIFF(YEAR, rc.BirthDate, GETDATE()) <= 60 AND rc.Sex = '1') THEN 'X' ELSE '' END AS Nam56Den60, " +
                   "CASE WHEN (rc.BirthDate IS NOT NULL AND((DATEDIFF(YEAR, rc.BirthDate, GETDATE()) > 55 AND rc.Sex = '1') OR " +
                   "(DATEDIFF(YEAR, rc.BirthDate, GETDATE()) > 60 AND rc.Sex = '1'))) THEN 'X' ELSE '' END AS TrenTuoiNghiHuu, " +
                   // Luan chuyen tu huyen
                   "'' AS LuanChuyenTuHuyen, " +
                   // Tong so
                   "0 AS 'xTongSo', " +
                   // Trong do
                   "0 AS 'xNu', " +
                   "0 AS 'xDangVien', " +
                   "0 AS 'xDanTocThieuSo', " +
                   "0 AS 'xTonGiao', " +
                   // Chuc danh
                   "0 AS 'xCanBoCapXa', " +
                   "0 AS 'xCongChucChuyenMon', " +
                   // Trinh do chuyen mon
                   "0 AS 'xTienSi', " +
                   "0 AS 'xThacSi', " +
                   "0 AS 'xDaiHoc', " +
                   "0 AS 'xCaoDang'," +
                   "0 AS 'xTrungCap', " +
                   "0 AS 'xSoCap', " +
                   // Trinh do van hoa
                   "0 AS 'xTHPT', " +
                   "0 AS 'xTHCS', " +
                   "0 AS 'xTieuHoc', " +
                   // Trinh do chinh tri
                   "0 as 'xCuNhanChinhTri', " +
                   "0 as 'xCaoCapChinhTri', " +
                   "0 as 'xTrungCapChinhTri', " +
                   "0 as 'xSoCapChinhTri', " +
                   // Trinh do tin hoc
                   "0 as 'xTrungCapTinHoc', " +
                   "0 as 'xChungChiTinHoc', " +
                   // Trinh do tieng Anh
                   "0 as 'xDaiHocTiengAnh', " +
                   "0 as 'xChungChiTiengAnh', " +
                   // Trinh do ngoai ngu khac
                   "0 as 'xDaiHocNgoaiNguKhac', " +
                   "0 as 'xChungChiNgoaiNguKhac', " +
                   // Trinh do tieng dan toc
                   "0 AS 'xChungChiTiengDanToc', " +
                   // Cap bac QLNN
                   "0 AS 'xChuyenVienCaoCap', " +
                   "0 AS 'xChuyenVienChinh', " +
                   "0 AS 'xChuyenVien', " +
                   "0 AS 'xChuaQuaDaoTao', " +
                   // Do tuoi
                   "0 AS 'xDuoi30', " +
                   "0 AS 'x31Den40', " +
                   "0 AS 'x41Den50', " +
                   "0 AS 'x51Den60', " +
                   "0 AS 'xNu51Den55', " +
                   "0 AS 'xNam56Den60', " +
                   "0 AS 'xTrenTuoiNghiHuu', " +
                   // Luan chuyen tu huyen
                   "0 AS 'xLuanChuyenTuHuyen' " +
                   "INTO #tmpB " +
                   "FROM hr_Record rc " +
                   "WHERE DepartmentId = @MaDonVi ";
            if(!string.IsNullOrEmpty(condition))
            {
                sql += " AND {0}".FormatWith(condition);
            }
            sql += "ORDER BY FullName ";

            // Tinh tong
            sql += "UPDATE	#tmpB " +
                   "SET " +
                   // Tong so
                   "xTongSo = xwTongSo, " +
                   // Trong do
                   "xNu = xwNu, " +
                   "xDangVien = xwDangVien, " +
                   "xDanTocThieuSo = xwDanTocThieuSo, " +
                   "xTonGiao = xwTonGiao, " +
                   // Chuc danh
                   "xCanBoCapXa = xwCanBoCapXa, " +
                   "xCongChucChuyenMon = xwCongChucChuyenMon, " +
                   // Trinh do chuyen mon
                   "xTienSi = xwTienSi, " +
                   "xThacSi = xwThacSi, " +
                   "xDaiHoc = xwDaiHoc, " +
                   "xCaoDang = xwCaoDang, " +
                   "xTrungCap = xwTrungCap, " +
                   "xSoCap = xwSoCap, " +
                   // Trinh do van hoa
                   "xTHPT = xwTHPT, " +
                   "xTHCS = xwTHCS, " +
                   "xTieuHoc = xwTieuHoc, " +
                   // Trinh do chinh tri
                   "xCuNhanChinhTri = xwCuNhanChinhTri, " +
                   "xCaoCapChinhTri = xwCaoCapChinhTri, " +
                   "xTrungCapChinhTri = xwTrungCapChinhTri, " +
                   "xSoCapChinhTri = xwSocapChinhTri, " +
                   // Trinh do tin hoc
                   "xTrungCapTinHoc = xwTrungCapTinHoc, " +
                   "xChungChiTinHoc = xwChungChiTinHoc, " +
                   // Trinh do tieng anh
                   "xDaiHocTiengAnh = xwDaiHocTiengAnh, " +
                   "xChungChiTiengAnh = xwChungChiTiengAnh, " +
                   // Trinh do ngoai ngu khac
                   "xDaiHocNgoaiNguKhac = xwDaiHocNgoaiNguKhac, " +
                   "xChungChiNgoaiNguKhac = xwChungChiNgoaiNguKhac, " +
                   // Trinh do tieng dan toc
                   "xChungChiTiengDanToc = xwChungChiTiengDanToc, " +
                   // Trinh do QLNN
                   "xChuyenVienCaoCap = xwChuyenVienCaoCap, " +
                   "xChuyenVienChinh = xwChuyenVienChinh, " +
                   "xChuyenVien = xwChuyenVien, " +
                   "xChuaQuaDaoTao = xwChuaQuaDaoTao, " +
                   // Do tuoi
                   "xDuoi30 = xwDuoi30, " +
                   "x31Den40 = xw31Den40, " +
                   "x41Den50 = xw41Den50, " +
                   "x51Den60 = xw51Den60, " +
                   "xNu51Den55 = xwNu51Den55, " +
                   "xNam56Den60 = xwNam56Den60, " +
                   "xTrenTuoiNghiHuu = xwTrenTuoiNghiHuu, " +
                   // Luan chuyen tu huyen
                   "xLuanChuyenTuHuyen = xwLuanChuyenTuHuyen " +
                   //
                   // From table
                   //
                   "FROM " +
                   "#tmpB " +
                   //
                   // Join table
                   //
                   "INNER JOIN " +
                   "(SELECT " +
                   // Tong so
                   "SUM(1) AS xwTongSo, " +
                   // Trong do
                   "SUM(CASE WHEN #tmpB.Nu = 'X' THEN 1 ELSE 0 END) AS xwNu, " +
                   "SUM(CASE WHEN #tmpB.DangVien = 'X' THEN 1 ELSE 0 END) AS xwDangVien, " +
                   "SUM(CASE WHEN #tmpB.DanTocThieuSo = 'X' THEN 1 ELSE 0 END) AS xwDanTocThieuSo, " +
                   "SUM(CASE WHEN #tmpB.TonGiao = 'X' THEN 1 ELSE 0 END) AS xwTongiao, " +
                   // Chuc danh
                   "SUM(CASE WHEN #tmpB.CanBoCapXa = 'X' THEN 1 ELSE 0 END) AS xwCanBoCapXa, " +
                   "SUM(CASE WHEN #tmpB.CongChucChuyenMon = 'X' THEN 1 ELSE 0 END) AS xwCongChucChuyenMon, " +
                   // Trinh do chuyen mon
                   "SUM(CASE WHEN #tmpB.TienSi = 'X' THEN 1 ELSE 0 END) AS xwTienSi, " +
                   "SUM(CASE WHEN #tmpB.ThacSi = 'X' THEN 1 ELSE 0 END) AS xwThacSi, " +
                   "SUM(CASE WHEN #tmpB.DaiHoc = 'X' THEN 1 ELSE 0 END) AS xwDaiHoc, " +
                   "SUM(CASE WHEN #tmpB.CaoDang = 'X' THEN 1 ELSE 0 END) AS xwCaoDang, " +
                   "SUM(CASE WHEN #tmpB.TrungCap = 'X' THEN 1 ELSE 0 END) AS xwTrungCap, " +
                   "SUM(CASE WHEN #tmpB.SoCap = 'X' THEN 1 ELSE 0 END) AS xwSoCap, " +
                   // Trinh do van hoa
                   "SUM(CASE WHEN #tmpB.THPT = 'X' THEN 1 ELSE 0 END) AS xwTHPT, " +
                   "SUM(CASE WHEN #tmpB.THCS = 'X' THEN 1 ELSE 0 END) AS xwTHCS, " +
                   "SUM(CASE WHEN #tmpB.TieuHoc = 'X' THEN 1 ELSE 0 END) AS xwTieuHoc, " +
                   // Trinh do chinh tri
                   "SUM(CASE WHEN #tmpB.CuNhanChinhTri = 'X' THEN 1 ELSE 0 END) AS xwCuNhanChinhTri, " +
                   "SUM(CASE WHEN #tmpB.CaoCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xwCaoCapChinhTri, " +
                   "SUM(CASE WHEN #tmpB.TrungCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xwTrungCapChinhTri, " +
                   "SUM(CASE WHEN #tmpB.SoCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xwSoCapChinhTri, " +
                   // Trinh do tin hoc
                   "SUM(CASE WHEN #tmpB.TrungCapTinHoc = 'X' THEN 1 ELSE 0 END) AS xwTrungCapTinHoc, " +
                   "SUM(CASE WHEN #tmpB.ChungChiTinHoc = 'X' THEN 1 ELSE 0 END) AS xwChungChiTinHoc, " +
                   // Trinh do tieng anh
                   "SUM(CASE WHEN #tmpB.DaiHocTiengAnh = 'X' THEN 1 ELSE 0 END) AS xwDaiHocTiengAnh, " +
                   "SUM(CASE WHEN #tmpB.ChungChiTiengAnh = 'X' THEN 1 ELSE 0 END) AS xwChungChiTiengAnh, " +
                   // Trinh do ngoai ngu khac
                   "SUM(CASE WHEN #tmpB.DaiHocNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xwDaiHocNgoaiNguKhac, " +
                   "SUM(CASE WHEN #tmpB.ChungChiNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xwChungChiNgoaiNguKhac, " +
                   // Trinh do tieng dan toc
                   "SUM(CASE WHEN #tmpB.ChungChiTiengDanToc = 'X' THEN 1 ELSE 0 END) AS xwChungChiTiengDanToc, " +
                   // Trinh do QLNN
                   "SUM(CASE WHEN #tmpB.ChuyenVienCaoCap = 'X' THEN 1 ELSE 0 END) AS xwChuyenVienCaoCap, " +
                   "SUM(CASE WHEN #tmpB.ChuyenVienChinh = 'X' THEN 1 ELSE 0 END) AS  xwChuyenVienChinh, " +
                   "SUM(CASE WHEN #tmpB.ChuyenVien = 'X' THEN 1 ELSE 0 END) AS xwChuyenVien, " +
                   "SUM(CASE WHEN #tmpB.ChuaQuaDaoTao = 'X' THEN 1 ELSE 0 END) AS xwChuaQuaDaoTao, " +
                   // Do tuoi
                   "SUM(CASE WHEN #tmpB.Duoi30 = 'X' THEN 1 ELSE 0 END) AS xwDuoi30, " +
                   "SUM(CASE WHEN #tmpB.Tu31Den40 = 'X' THEN 1 ELSE 0 END) AS xw31Den40, " +
                   "SUM(CASE WHEN #tmpB.Tu41Den50 = 'X' THEN 1 ELSE 0 END) AS xw41Den50, " +
                   "SUM(CASE WHEN #tmpB.Tu51Den60 = 'X' THEN 1 ELSE 0 END) AS xw51Den60, " +
                   "SUM(CASE WHEN #tmpB.Nu51Den55 = 'X' THEN 1 ELSE 0 END) AS xwNu51Den55, " +
                   "SUM(CASE WHEN #tmpB.Nam56Den60 = 'X' THEN 1 ELSE 0 END) AS xwNam56Den60, " +
                   "SUM(CASE WHEN #tmpB.TrenTuoiNghiHuu = 'X' THEN 1 ELSE 0 END) AS xwTrenTuoiNghiHuu, " +
                   // Ngach CCVC
                   "SUM(CASE WHEN #tmpB.LuanChuyenTuHuyen = 'X' THEN 1 ELSE 0 END) AS xwLuanChuyenTuHuyen " +
                   //
                   // From table
                   //
                   "FROM #tmpB) AS TMP " +
                   "ON 1=1 ";

            sql += "SELECT * FROM #tmpB ";

            // return
            return sql;
        }

        /// <summary>
        /// Danh sách và tiền lương cán bộ, công chức cấp huyện
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_SalaryDistrictCivilServants(string departments, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            //create sql
            // xoa bang #tmpB neu da ton tai
            sql += "IF OBJECT_ID('tempdb..#tmpB') IS NOT NULL DROP Table #tempB ";
            sql += "	SELECT 	" +
                   "	FullName AS HoTen," +
                   "   DepartmentId AS MaDonVi, " +
                   "	CASE WHEN Sex = '1' THEN CONVERT( varchar,H.BirthDate, 103) ELSE '' END AS Nam," +
                   "	CASE WHEN Sex = '0' THEN CONVERT( varchar,H.BirthDate, 103) ELSE '' END AS Nu," +
                   "	CASE WHEN H.PositionId IS NOT NULL THEN	" +
                   "		(SELECT TOP 1 Name FROM cat_Position DC WHERE DC.Id = H.PositionId)" +
                   "		ELSE " +
                   "		(SELECT TOP 1 Name FROM cat_JobTitle DCV WHERE DCV.Id = H.JobTitleId)" +
                   "		END AS ChucVu," +
                   "	(SELECT TOP 1 Name FROM cat_Department DD WHERE DD.Id = H.DepartmentId) AS CoQuan,	" +
                   "   DATEDIFF(month,(SELECT TOP 1 EffectiveDate  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC), getDate()) AS ThoiGianGiuChucVu," +
                   //Muc luong hien huong
                   "	(SELECT TOP 1 SalaryFactor  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC ) AS HeSoLuong," +
                   "	(select TOP 1 cq.Code  from sal_SalaryDecision hl										" +
                   "	left join cat_Quantum cq on hl.QuantumId = cq.Id										" +
                   "	where hl.RecordId = H.Id										" +
                   "	and hl.DecisionDate = (select MAX(hs2.DecisionDate) from sal_SalaryDecision hs2 where hs2.RecordId = H.Id))	as MaSoNgach, " +

                   //Phu cap
                   //"	(SELECT TOP 1 PositionAllowance  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS PhuCapChucVu," +
                   "   (SELECT TOP 1 ResponsibilityAllowance FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS PhuCapTrachNhiem," +
                   "   (SELECT TOP 1 AreaAllowance FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS PhuCapKhuVuc," +
                   "	(SELECT TOP 1 OverGrade  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS PhuCapVuotKhung," +
                   //"	(SELECT TOP 1 (((PositionAllowance + ResponsibilityAllowance + AreaAllowance)/ SalaryFactor)*100 + OverGrade)" +
                   "         FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id AND HL.SalaryFactor IS NOT NULL AND HL.SalaryFactor > 0 " +
                   "         ORDER BY HL.ID DESC) AS TongPhuCapPhanTram," +
                   //Ghi chu
                   "   (SELECT TOP 1 Note  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS GhiChu," +
                   //Tong
                   "	cast('0' as float) AS 'xHeSoLuong'," +
                   "	cast('0' as float) AS 'xPhuCapChucVu'," +
                   "	cast('0' as float) AS 'xPhuCapVuotKhung'," +
                   "	cast('0' as float) AS 'xTongPhuCapPhanTram'," +
                   "   cast('0' as float) AS 'xPhuCapTrachNhiem'," +
                   "   cast('0' as float) AS 'xPhuCapKhuVuc'" +
                   "	INTO #tempB	" +
                   "	FROM hr_Record H	" +
                   "	WHERE H.DepartmentId IN ({0})".FormatWith(departments) +
                   "	ORDER BY H.FullName " +

                   "	UPDATE #tempB " +
                   "	SET " +
                   "	xHeSoLuong = xwHeSoLuong," +
                   "	xPhuCapChucVu = xwPhuCapChucVu," +
                   "	xPhuCapVuotKhung = xwPhuCapVuotKhung," +
                   "	xTongPhuCapPhanTram = xwTongPhuCapPhanTram," +
                   "   xPhuCapTrachNhiem = xwPhuCapTrachNhiem," +
                   "   xPhuCapKhuVuc = xwPhuCapKhuVuc" +
                   "	FROM #tempB " +
                   "	INNER JOIN(SELECT " +
                   "	SUM(ISNULL(HeSoLuong, 0)) AS xwHeSoLuong," +
                   "	SUM(ISNULL(PhuCapChucVu,0)) AS xwPhuCapChucVu," +
                   "	SUM(ISNULL(PhuCapVuotKhung,0)) AS xwPhuCapVuotKhung," +
                   "	SUM(ISNULL(TongPhuCapPhanTram,0)) AS xwTongPhuCapPhanTram, " +
                   "   SUM(ISNULL(PhuCapTrachNhiem,0)) AS xwPhuCapTrachNhiem," +
                   "   SUM(ISNULL(PhuCapKhuVuc,0)) AS xwPhuCapKhuVuc " +
                   "	FROM #tempB " +
                   "	) AS TMP ON 1=1	" +
                   "	SELECT * FROM #tempB " +
                   "   WHERE #tempB.HeSoLuong IS NOT NULL ";
            if(!string.IsNullOrEmpty(condition))
            {
                sql += " AND {0}".FormatWith(condition);
            }
            sql += "   ORDER BY #tempB.MaDonVi, #tempB.HoTen";

            return sql;
        }

        /// <summary>
        /// Danh sách và tiền lương cán bộ, công chức cấp xã
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_SalarycommuneCivilServants(string departments, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            //create SQL
            // xoa bang #tmpB neu da ton tai
            sql += "IF OBJECT_ID('tempdb..#tempB') IS NOT NULL DROP Table #tempB ";
            sql += "SELECT " +
                   " FullName AS HoTen," +
                   "CASE WHEN Sex = '1' THEN CONVERT( varchar,H.BirthDate, 103) ELSE '' END AS Nam," +
                   "CASE WHEN Sex = '0' THEN CONVERT( varchar,H.BirthDate, 103) ELSE '' END AS Nu," +
                   "CASE WHEN H.PositionId IS NOT NULL THEN" +
                   "   (SELECT TOP 1 Name FROM cat_Position DC WHERE DC.Id = H.PositionId)" +
                   "  ELSE" +
                   " (SELECT TOP 1 Name FROM cat_JobTitle DCV WHERE DCV.Id = H.JobTitleId)" +
                   "END AS ChucVu," +
                   " (SELECT TOP 1 Name FROM cat_Department DD WHERE DD.Id = H.DepartmentId) AS CoQuan," +
                   " DATEDIFF(month,(SELECT TOP 1 EffectiveDate  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC), getDate()) AS ThoiGianGiuChucVu," +
                   " (SELECT TOP 1 SalaryFactor  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS HeSoLuong," +
                   " (SELECT TOP 1 SalaryGrade  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS BacLuong," +
                   //" (SELECT TOP 1 PositionAllowance  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS PhuCapChucVu," +
                   " (SELECT TOP 1 ResponsibilityAllowance FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS PhuCapTrachNhiem," +
                   " (SELECT TOP 1 AreaAllowance FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS PhuCapKhuVuc," +
                   " (SELECT TOP 1 OverGrade  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS PhuCapVuotKhung," +
                   //" (SELECT TOP 1 (((PositionAllowance + ResponsibilityAllowance + AreaAllowance)/ SalaryFactor)*100 + OverGrade)  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id AND HL.SalaryFactor IS NOT NULL AND HL.SalaryFactor > 0  ORDER BY HL.ID DESC) AS TongPhuCapPhanTram," +
                   " (SELECT TOP 1 Note  FROM sal_SalaryDecision HL WHERE HL.RecordId = H.Id ORDER BY HL.ID DESC) AS GhiChu," +
                   " cast('0' as float) AS 'xHeSoLuong'," +
                   " cast('0' as float) AS 'xPhuCapChucVu'," +
                   " cast('0' as float) AS 'xPhuCapVuotKhung'," +
                   " cast('0' as float) AS 'xTongPhuCapPhanTram'," +
                   " cast('0' as float) AS 'xPhuCapTrachNhiem'," +
                   " cast('0' as float) AS 'xPhuCapKhuVuc'" +
                   " INTO #tempB " +
                   " FROM hr_Record H " +
                   " WHERE H.DepartmentId = @MaDonVi " +
                   " ORDER BY H.FullName " +
                   " UPDATE #tempB " +
                   " SET " +
                   " xHeSoLuong = xwHeSoLuong," +
                   " xPhuCapChucVu = xwPhuCapChucVu," +
                   " xPhuCapVuotKhung = xwPhuCapVuotKhung," +
                   " xTongPhuCapPhanTram = xwTongPhuCapPhanTram," +
                   " xPhuCapTrachNhiem = xwPhuCapTrachNhiem," +
                   " xPhuCapKhuVuc = xwPhuCapKhuVuc" +
                   " FROM #tempB " +
                   "  INNER JOIN(SELECT" +
                   "  SUM(ISNULL(HeSoLuong, 0)) AS xwHeSoLuong," +
                   "  SUM(ISNULL(PhuCapChucVu,0)) AS xwPhuCapChucVu," +
                   "  SUM(ISNULL(PhuCapVuotKhung,0)) AS xwPhuCapVuotKhung," +
                   "  SUM(ISNULL(TongPhuCapPhanTram,0)) AS xwTongPhuCapPhanTram," +
                   "  SUM(ISNULL(PhuCapTrachNhiem,0)) AS xwPhuCapTrachNhiem," +
                   "  SUM(ISNULL(PhuCapKhuVuc,0)) AS xwPhuCapKhuVuc" +
                   "  FROM #tempB" +
                   " ) AS TMP ON 1=1 " +
                   "SELECT * FROM #tempB " +
                   " WHERE #tempB.HeSoLuong IS NOT NULL";
            if(!string.IsNullOrEmpty(condition))
            {
                sql += " AND {0}".FormatWith(condition);
            }

            return sql;
        }

        /// <summary>
        /// Số lượng, chất lượng cán bộ, công chức cấp huyện chi tiết
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_QuantyDistrictDetail(string departments, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            // Xoa bang #tmpB neu da ton tai
            sql += "IF OBJECT_ID('tempdb..#tmpB') IS NOT NULL DROP Table #tmpB ";
            // Tong hop
            sql += " select h.departmentId, h.fullname,d.Name as DepartmentName,h.RecruimentDepartment, " +
                   " case when h.Sex = '0' then 'X' end as Nu, " +
                   " case when h.CPVJoinedDate is not null then 'X' end as DangVien, " +
                   " case when dt.IsMinority = 1 then 'X'end as DanTocThieuSo ," +
                   " case when h.ReligionId IS NOT NULL and tg.Name like N'%Không%' then 'X' end as TonGiao ," +
                   " case when cm.[Group] = 'TS' then 'X' end as TienSi, " +
                   " case when cm.[Group] = 'ThS' then 'X' end as ThacSi," +
                   " case when cm.[Group] = 'DH' then 'X' end as DaiHoc," +
                   " case when cm.[Group] = 'CD' then 'X' end as CaoDang," +
                   " case when cm.[Group] = 'TC' then 'X' end as TrungCap," +
                   " case when cm.[Group] = 'SC' then 'X' end as SoCap," +
                   " case when dtc.[Group] = 'CN' then 'X' end as CuNhanChinhTri," +
                   " case when dtc.[Group] = 'CC' then 'X' end as CaoCapChinhTri," +
                   " case when dtc.[Group] = 'TC' then 'X' end as TrungCapChinhTri," +
                   " case when dtc.[Group] = 'SC' then 'X' end as SoCapChinhTri," +
                   " case when dth.[Group] = 'TC' then 'X' end as TrungCapTinHoc," +
                   " case when dth.[Group] = 'CC' then 'X' end as ChungChiTinHoc," +
                   " case when h.LanguageLevelId<> 0 and dnn.[Group] = 'DHTA' then 'X' end as DaiHocTiengAnh," +
                   " case when h.LanguageLevelId<> 0 and dnn.[Group] = 'CCTA' then 'X' end as ChungChiTiengAnh," +
                   " case when h.LanguageLevelId<> 0 and dnn.[Group] = 'DHNNK' then 'X' end as DaiHocNgoaiNguKhac," +
                   " case when h.LanguageLevelId<> 0 and dnn.[Group] = 'CCNNK' then 'X' end as ChungChiNgoaiNguKhac," +
                   " case when h.LanguageLevelId<> 0 and dnn.[Group] = 'CCTDT' then 'X' end as ChungChiTiengDanToc," +
                   " case when dtq.[Group] = 'CVCC' then 'X' end as ChuyenVienCaoCap," +
                   " case when dtq.[Group] = 'CVC' then 'X' end as ChuyenVienChinh," +
                   " case when dtq.[Group] = 'CV' then 'X' end as ChuyenVien," +
                   " case when h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE())<= 30 then 'X' end as Duoi30, " +
                   " case when h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE())> 30 AND DATEDIFF(YEAR, h.BirthDate, GETDATE())<= 40  then 'X' end as Tu31Den40, " +
                   " case when h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE())> 40 AND DATEDIFF(YEAR, h.BirthDate, GETDATE())<= 50  then 'X' end as Tu41Den50, " +
                   " case when h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE())> 50 AND DATEDIFF(YEAR, h.BirthDate, GETDATE())<= 55 AND h.Sex = '0' then 'X' end as Nu51Den55,   " +
                   " case when h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE())> 55 AND DATEDIFF(YEAR, h.BirthDate, GETDATE())<= 60 AND h.Sex = '1' then 'X' end as Nam56Den60,  " +
                   " case when h.BirthDate IS NOT NULL AND ((DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 55 AND h.Sex = '0') OR(DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 60 AND h.Sex = '1')) then 'X' end as TrenTuoiNghiHuu,  " +
                   " case when s.QuantumId IN (SELECT dn.Id FROM cat_Quantum dn, cat_GroupQuantum nn WHERE dn.GroupQuantumId = nn.Id AND nn.[Group] = 'CVCC') then 'X' end as NgachCVCC,                  " +
                   " case when s.QuantumId IN (SELECT dn.Id FROM cat_Quantum dn, cat_GroupQuantum nn WHERE dn.GroupQuantumId = nn.Id AND nn.[Group] = 'CVC') then 'X' end as NgachCVC,                    " +
                   " case when s.QuantumId IN (SELECT dn.Id FROM cat_Quantum dn, cat_GroupQuantum nn WHERE dn.GroupQuantumId = nn.Id AND nn.[Group] = 'CV') then 'X' end as NgachCV,                          " +
                   " case when s.QuantumId IN (SELECT dn.Id FROM cat_Quantum dn, cat_GroupQuantum nn WHERE dn.GroupQuantumId = nn.Id AND nn.[Group] = 'CS') then 'X' end as NgachCS,                               " +
                   " case when s.QuantumId IN (SELECT dn.Id FROM cat_Quantum dn, cat_GroupQuantum nn WHERE dn.GroupQuantumId = nn.Id AND nn.[Group] = 'NV') then 'X' end as NgachNV,                             " +

                   // Trong do
                   "0 AS 'xNu', " +
                   "0 AS 'xDangVien', " +
                   "0 AS 'xDanTocThieuSo', " +
                   "0 AS 'xTonGiao', " +
                   // Trinh do chuyen mon
                   "0 AS 'xTienSi', " +
                   "0 AS 'xThacSi', " +
                   "0 AS 'xDaiHoc', " +
                   "0 AS 'xCaoDang'," +
                   "0 AS 'xTrungCap', " +
                   "0 AS 'xSoCap', " +
                   // Trinh do chinh tri
                   "0 as 'xCuNhanChinhTri', " +
                   "0 as 'xCaoCapChinhTri', " +
                   "0 as 'xTrungCapChinhTri', " +
                   "0 as 'xSoCapChinhTri', " +
                   // Trinh do tin hoc
                   "0 as 'xTrungCapTinHoc', " +
                   "0 as 'xChungChiTinHoc', " +
                   // Trinh do tieng Anh
                   "0 as 'xDaiHocTiengAnh', " +
                   "0 as 'xChungChiTiengAnh', " +
                   // Trinh do ngoai ngu khac
                   "0 as 'xDaiHocNgoaiNguKhac', " +
                   "0 as 'xChungChiNgoaiNguKhac', " +
                   //Chung chi tieng dan toc
                   "0 as 'xChungChiTiengDanToc', " +
                   // Cap bac QLNN
                   "0 AS 'xChuyenVienCaoCap', " +
                   "0 AS 'xChuyenVienChinh', " +
                   "0 AS 'xChuyenVien', " +
                   // Do tuoi
                   "0 AS 'xDuoi30', " +
                   "0 AS 'x31Den40', " +
                   "0 AS 'x41Den50', " +
                   "0 AS 'xNu51Den55', " +
                   "0 AS 'xNam56Den60', " +
                   "0 AS 'xTrenTuoiNghiHuu', " +
                   // Ngach CCVC
                   "0 AS 'xNgachCVCC', " +
                   "0 AS 'xNgachCVC', " +
                   "0 AS 'xNgachCV', " +
                   "0 AS 'xNgachCS', " +
                   "0 AS 'xNgachNV' " +
                   // Day du lieu vao bang #tmpB
                   "INTO #tmpB " +

                   " from hr_Record h " +
                   " left join cat_Folk dt ON dt.Id = h.FolkId " +
                   "left join cat_Department d on h.DepartmentId = d.Id " +
                   " left join cat_Religion tg on tg.id = h.ReligionId " +
                   " LEFT JOIN cat_Education cm ON dt.Id = h.EducationId " +
                   " LEFT JOIN cat_PoliticLevel dtc on dtc.Id = h.PoliticLevelId " +
                   " LEFT JOIN cat_ITLevel dth on dth.Id = h.ITLevelId " +
                   " LEFT JOIN cat_LanguageLevel dnn ON dnn.Id = h.LanguageLevelId " +
                   " LEFT JOIN(SELECT RecordId, QuantumId FROM sal_SalaryDecision GROUP BY RecordId, QuantumId) s ON h.Id = s.RecordId " +
                   " LEFT JOIN cat_ManagementLevel dtq ON dtq.Id = h.ManagementLevelId " +
                   " WHERE h.DepartmentId IN ({0})".FormatWith(departments);

            // Tinh tong
            sql += "UPDATE	#tmpB " +
                   "SET " +
                   // Trong do
                   "xNu = xwNu, " +
                   "xDangVien = xwDangVien, " +
                   "xDanTocThieuSo = xwDanTocThieuSo, " +
                   "xTonGiao = xwTonGiao, " +
                   // Tring do chuyen mon
                   "xTienSi = xwTienSi, " +
                   "xThacSi = xwThacSi, " +
                   "xDaiHoc = xwDaiHoc, " +
                   "xCaoDang = xwCaoDang, " +
                   "xTrungCap = xwTrungCap, " +
                   "xSoCap = xwSoCap, " +
                   // Trinh do chinh tri
                   "xCuNhanChinhTri = xwCuNhanChinhTri, " +
                   "xCaoCapChinhTri = xwCaoCapChinhTri, " +
                   "xTrungCapChinhTri = xwTrungCapChinhTri, " +
                   "xSoCapChinhTri = xwSocapChinhTri, " +
                   // Trinh do tin hoc
                   "xTrungCapTinHoc = xwTrungCapTinHoc, " +
                   "xChungChiTinHoc = xwChungChiTinHoc, " +
                   // Trinh do tieng anh
                   "xDaiHocTiengAnh = xwDaiHocTiengAnh, " +
                   "xChungChiTiengAnh = xwChungChiTiengAnh, " +
                   // Trinh do ngoai ngu khac
                   "xDaiHocNgoaiNguKhac = xwDaiHocNgoaiNguKhac, " +
                   "xChungChiNgoaiNguKhac = xwChungChiNgoaiNguKhac, " +
                   //Chung chi tieng dan toc
                   "xChungChiTiengDanToc = xwChungChiTiengDanToc, " +
                   // Trinh do QLNN
                   "xChuyenVienCaoCap = xwChuyenVienCaoCap, " +
                   "xChuyenVienChinh = xwChuyenVienChinh, " +
                   "xChuyenVien = xwChuyenVien, " +
                   // Do tuoi
                   "xDuoi30 = xwDuoi30, " +
                   "x31Den40 = xw31Den40, " +
                   "x41Den50 = xw41Den50, " +
                   "xNu51Den55 = xwNu51Den55, " +
                   "xNam56Den60 = xwNam56Den60, " +
                   "xTrenTuoiNghiHuu = xwTrenTuoiNghiHuu, " +
                   // Ngach CCVC
                   "xNgachCVCC = xwNgachCVCC, " +
                   "xNgachCVC = xwNgachCVC, " +
                   "xNgachCV = xwNgachCV, " +
                   "xNgachCS = xwNgachCS, " +
                   "xNgachNV = xwNgachNV " +
                   //
                   // From table
                   //
                   "FROM " +
                   "#tmpB " +
                   //
                   // Join table
                   "INNER JOIN " +
                   "(SELECT " +
                   // Trong do
                   "SUM(CASE WHEN #tmpB.Nu = 'X' THEN 1 ELSE 0 END) AS xwNu, " +
                   "SUM(CASE WHEN #tmpB.DangVien = 'X' THEN 1 ELSE 0 END) AS xwDangVien, " +
                   "SUM(CASE WHEN #tmpB.DanTocThieuSo  = 'X' THEN 1 ELSE 0  END) AS xwDanTocThieuSo, " +
                   "SUM(CASE WHEN #tmpB.TonGiao = 'X' THEN 0 ELSE 0 END) AS xwTongiao, " +
                   // Trinh do chuyen mon
                   "SUM(CASE WHEN #tmpB.TienSi = 'X' THEN 1 ELSE 0 END) AS xwTienSi, " +
                   "SUM(CASE WHEN #tmpB.ThacSi = 'X' THEN 1 ELSE 0 END) AS xwThacSi, " +
                   "SUM(CASE WHEN #tmpB.DaiHoc = 'X' THEN 1 ELSE 0 END) AS xwDaiHoc, " +
                   "SUM(CASE WHEN #tmpB.CaoDang = 'X' THEN 1 ELSE 0 END) AS xwCaoDang, " +
                   "SUM(CASE WHEN #tmpB.TrungCap = 'X' THEN 1 ELSE 0 END ) AS xwTrungCap, " +
                   "SUM(CASE WHEN #tmpB.SoCap = 'X' THEN 1 ELSE  0 END) AS xwSoCap, " +
                   // Trinh do chinh tri
                   "SUM(CASE WHEN #tmpB.CuNhanChinhTri = 'X' THEN 1 ELSE 0 END) AS xwCuNhanChinhTri, " +
                   "SUM(CASE WHEN #tmpB.CaoCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xwCaoCapChinhTri, " +
                   "SUM(CASE WHEN #tmpB.TrungCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xwTrungCapChinhTri, " +
                   "SUM(CASE WHEN #tmpB.SoCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xwSoCapChinhTri, " +
                   // Trinh do tin hoc
                   "SUM(CASE WHEN #tmpB.TrungCapTinHoc = 'X' THEN 1 ELSE 0 END) AS xwTrungCapTinHoc, " +
                   "SUM(CASE WHEN #tmpB.ChungChiTinHoc = 'X' THEN 1 ELSE 0 END )AS xwChungChiTinHoc, " +
                   // Trinh do tieng anh
                   "SUM(CASE WHEN #tmpB.DaiHocTiengAnh = 'X' THEN 1 ELSE 0 END) AS xwDaiHocTiengAnh, " +
                   "SUM(CASE WHEN #tmpB.ChungChiTiengAnh = 'X' THEN 1 ELSE 0 END ) AS xwChungChiTiengAnh, " +
                   // Trinh do ngoai ngu khac
                   "SUM(CASE WHEN #tmpB.DaiHocNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xwDaiHocNgoaiNguKhac, " +
                   "SUM(CASE WHEN #tmpB.ChungChiNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xwChungChiNgoaiNguKhac, " +
                   //Chung chi tieng dan toc
                   "SUM(CASE WHEN #tmpB.ChungChiTiengDanToc = 'X' THEN 1 ELSE 0 END) AS xwChungChiTiengDanToc, " +
                   // Trinh do QLNN
                   "SUM(CASE WHEN #tmpB.ChuyenVienCaoCap = 'X' THEN 1 ELSE 0 END) AS xwChuyenVienCaoCap, " +
                   "SUM(CASE WHEN #tmpB.ChuyenVienChinh = 'X' THEN 1 ELSE 0  END) AS  xwChuyenVienChinh, " +
                   "SUM(CASE WHEN #tmpB.ChuyenVien = 'X' THEN 1 ELSE 0  END) AS xwChuyenVien, " +
                   // Do tuoi
                   "SUM(CASE WHEN #tmpB.Duoi30 = 'X' THEN 1 ELSE 0 END) AS xwDuoi30, " +
                   "SUM(CASE WHEN #tmpB.Tu31Den40 = 'X' THEN 1 ELSE 0 END) AS xw31Den40, " +
                   "SUM(CASE WHEN #tmpB.Tu41Den50 = 'X' THEN 1 ELSE 0 END) AS xw41Den50, " +
                   "SUM(CASE WHEN #tmpB.Nu51Den55 = 'X' THEN 1 ELSE 0 END) AS xwNu51Den55, " +
                   "SUM(CASE WHEN #tmpB.Nam56Den60 = 'X' THEN 1 ELSE 0 END) AS xwNam56Den60, " +
                   "SUM(CASE WHEN #tmpB.TrenTuoiNghiHuu = 'X' THEN 1 ELSE 0 END) AS xwTrenTuoiNghiHuu, " +
                   // Ngach CCVC
                   "SUM(CASE WHEN #tmpB.NgachCVCC = 'X' THEN 1 ELSE 0  END) AS xwNgachCVCC, " +
                   "SUM(CASE WHEN #tmpB.NgachCVC = 'X' THEN 1 ELSE  0 END) AS xwNgachCVC, " +
                   "SUM(CASE WHEN #tmpB.NgachCV = 'X' THEN 1 ELSE 0 END) AS xwNgachCV, " +
                   "SUM(CASE WHEN #tmpB.NgachCS = 'X' THEN 1 ELSE 0  END) AS xwNgachCS, " +
                   "SUM(CASE WHEN #tmpB.NgachNV = 'X' THEN 1 ELSE 0 END) AS xwNgachNV " +
                   //
                   // From table
                   //
                   "FROM #tmpB) AS TMP " +
                   " ON 1=1 ";
            // select all from #tmpB
            sql += "SELECT * FROM #tmpB ";

            // return
            return sql;
        }

        /// <summary>
        /// Số lượng, chất lượng người làm việc và biên chế
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="kindTypeReport"></param>
        /// <param name="employeeType"></param>
        /// <returns></returns>
        public static string GetStore_CountGroupTotal(string departments, string kindTypeReport, string employeeType)
        {
            var sql = string.Empty;
            sql += "	IF OBJECT_ID('tempdb..#tmpBGroup') IS NOT NULL DROP Table #tmpBGroup 		" +
                   "SELECT 		" +
                   "h.EmployeeTypeId AS EmployeeTypeId,		" +
                   "(SELECT TOP 1 emp.Name	FROM cat_EmployeeType emp WHERE emp.Id = h.EmployeeTypeId) AS TenLoaiCanBo,	" +
                   "(SELECT dd.Name FROM cat_Department dd WHERE dd.Id = h.DepartmentId ) AS 'TenDonVi', 		" +
                   "CASE WHEN (h.CPVOfficialJoinedDate IS NOT NULL) THEN 'X' ELSE '' END AS DangVien, 		" +
                   "CASE WHEN (SELECT TOP 1 IsMinority FROM cat_Folk WHERE Id = h.FolkId) = '1' THEN 'X' ELSE '' END AS DanTocThieuSo, 		" +
                   "CASE WHEN h.Sex = '0' THEN 'X' ELSE '' END AS Nu, 		" +
                   "CASE WHEN h.Sex = '1' THEN 'X' ELSE '' END AS Nam,	" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 30) THEN 'X' ELSE '' END AS Duoi30, 		" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 30 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 40) THEN 'X' ELSE '' END AS Tu31Den40, 		" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 40 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 50) THEN 'X' ELSE '' END AS Tu41Den50, 		" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 50 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 55 AND h.Sex = '0') THEN 'X' ELSE '' END AS Nu51Den55, 		" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 50 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 55 AND h.Sex = '1') THEN 'X' ELSE '' END AS Nam51Den55,		" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 55 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 60 AND h.Sex = '1') THEN 'X' ELSE '' END AS Nam56Den60, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CVC' THEN 'X' ELSE '' END AS ChuyenVienChinh, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CV' THEN 'X' ELSE '' END AS ChuyenVien, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CS' THEN 'X' ELSE '' END AS CanSu, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CQDT' THEN 'X' ELSE '' END AS NgachConLai, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_BasicEducation WHERE Id = h.BasicEducationId) = 'THCS' THEN 'X' ELSE '' END AS THCS, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_BasicEducation WHERE Id = h.BasicEducationId) = 'THPT' THEN 'X' ELSE '' END AS THPT,		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = h.EducationId) = 'ThS' THEN 'X' ELSE '' END AS ThacSi, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = h.EducationId) = 'DH' THEN 'X' ELSE '' END AS DaiHoc, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = h.EducationId) = 'CD' THEN 'X' ELSE '' END AS CaoDang, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = h.EducationId) = 'TC' THEN 'X' ELSE '' END AS TrungCap, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = h.EducationId) = 'SC' OR (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'TS' THEN 'X' ELSE '' END AS TrinhDoChuyenMonConLai, 		" +
                   "CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = h.PoliticLevelId) = '4' THEN 'X' ELSE '' END AS CuNhanChinhTri, 		" +
                   "CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = h.PoliticLevelId) = '3' THEN 'X' ELSE '' END AS CaoCapChinhTri, 		" +
                   "CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = h.PoliticLevelId) = '2' THEN 'X' ELSE '' END AS TrungCapChinhTri, 		" +
                   "'' AS ChuongTrinhChuyenVien,		" +
                   "'' AS QLGD,		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = h.ITLevelId) = 'DH' THEN 'X' ELSE '' END AS DaiHocTinHoc, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = h.ITLevelId) = 'CD' THEN 'X' ELSE '' END AS CaoDangTinHoc, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = h.ITLevelId) = 'TC' THEN 'X' ELSE '' END AS TrungCapTinHoc, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = h.ITLevelId) = 'CC' THEN 'X' ELSE '' END AS ChungChiTinHoc, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = h.LanguageLevelId) = 'DHTA' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId  WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'DHTA') = 'DHTA' THEN 'X' ELSE '' END AS DaiHocTiengAnh,	" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = h.LanguageLevelId) = 'CCTA' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId  WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'CCTA') = 'CCTA' THEN 'X' ELSE '' END AS ChungChiTiengAnh,	" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = h.LanguageLevelId) = 'DHNNK' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'DHNNK') = 'DHNNK' THEN 'X' ELSE '' END AS DaiHocNgoaiNguKhac,	" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = h.LanguageLevelId) = 'CCNNK' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'CCNNK') = 'CCNNK' THEN 'X' ELSE '' END AS ChungChiNgoaiNguKhac, 	" +
                   " '' AS GhiChu " +
                   "INTO #tmpBGroup 		" +
                   "FROM hr_Record h 		" +
                   "   LEFT JOIN cat_EmployeeType ce ON ce.Id = h.EmployeeTypeId " +
                   "WHERE h.DepartmentId IN ({0}) ".FormatWith(departments);
            if("LaNu" == kindTypeReport)
            {
                sql += "AND h.Sex = '0'  ";
            }
            if(!string.IsNullOrEmpty(employeeType))
            {
                sql += " AND ce.[Group] = '{0}' ".FormatWith(employeeType);
            }
            sql += "ORDER BY h.EmployeeTypeId 		" +
                   "SELECT  " +
                   "MAX(#tmpBGroup.TenLoaiCanBo) AS TenLoaiCanBo, " +
                   "SUM(CASE WHEN #tmpBGroup.DangVien = 'X' THEN 1 ELSE 0 END) AS xGroupDangVien,	" +
                   "SUM(CASE WHEN #tmpBGroup.DanTocThieuSo = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocThieuSo, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Nu = 'X' THEN 1 ELSE 0 END) AS xGroupNu,	" +
                   "SUM(CASE WHEN #tmpBGroup.Nam = 'X' THEN 1 ELSE 0 END) AS xGroupNam, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Duoi30 = 'X' THEN 1 ELSE 0 END) AS xGroupDuoi30, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Tu31Den40 = 'X' THEN 1 ELSE 0 END) AS xGroup31Den40, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Tu41Den50 = 'X' THEN 1 ELSE 0 END) AS xGroup41Den50, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Nu51Den55 = 'X' THEN 1 ELSE 0 END) AS xGroupNu51Den55, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Nam51Den55 = 'X' THEN 1 ELSE 0 END) AS xGroupNam51Den55, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Nam56Den60 = 'X' THEN 1 ELSE 0 END) AS xGroupNam56Den60, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ThacSi = 'X' THEN 1 ELSE 0 END) AS xGroupThacSi, 	" +
                   "SUM(CASE WHEN #tmpBGroup.DaiHoc = 'X' THEN 1 ELSE 0 END) AS xGroupDaiHoc, 	" +
                   "SUM(CASE WHEN #tmpBGroup.CaoDang = 'X' THEN 1 ELSE 0 END) AS xGroupCaoDang, 	" +
                   "SUM(CASE WHEN #tmpBGroup.TrungCap = 'X' THEN 1 ELSE 0 END) AS xGroupTrungCap, 	" +
                   "SUM(CASE WHEN #tmpBGroup.TrinhDoChuyenMonConLai = 'X' THEN 1 ELSE 0 END) AS xGroupTrinhDoChuyenMonConLai, 	" +
                   "SUM(CASE WHEN #tmpBGroup.THCS = 'X' THEN 1 ELSE 0 END) AS xGroupTHCS, 	" +
                   "SUM(CASE WHEN #tmpBGroup.THPT = 'X' THEN 1 ELSE 0 END) AS xGroupTHPT, 	" +
                   "SUM(CASE WHEN #tmpBGroup.CuNhanChinhTri = 'X' THEN 1 ELSE 0 END) AS xGroupCuNhanChinhTri, 	" +
                   "SUM(CASE WHEN #tmpBGroup.CaoCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xGroupCaoCapChinhTri, 	" +
                   "SUM(CASE WHEN #tmpBGroup.TrungCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xGroupTrungCapChinhTri, 	" +
                   "SUM(CASE WHEN #tmpBGroup.DaiHocTinHoc = 'X' THEN 1 ELSE 0 END) AS xGroupDaiHocTinHoc, 	" +
                   "SUM(CASE WHEN #tmpBGroup.CaoDangTinHoc = 'X' THEN 1 ELSE 0 END) AS xGroupCaoDangTinHoc, 	" +
                   "SUM(CASE WHEN #tmpBGroup.TrungCapTinHoc = 'X' THEN 1 ELSE 0 END) AS xGroupTrungCapTinHoc, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ChungChiTinHoc = 'X' THEN 1 ELSE 0 END) AS xGroupChungChiTinHoc, 	" +
                   "SUM(CASE WHEN #tmpBGroup.DaiHocTiengAnh = 'X' THEN 1 ELSE 0 END) AS xGroupDaiHocTiengAnh, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ChungChiTiengAnh = 'X' THEN 1 ELSE 0 END) AS xGroupChungChiTiengAnh, 	" +
                   "SUM(CASE WHEN #tmpBGroup.DaiHocNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xGroupDaiHocNgoaiNguKhac, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ChungChiNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xGroupChungChiNgoaiNguKhac, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ChuyenVienChinh = 'X' THEN 1 ELSE 0 END) AS  xGroupChuyenVienChinh, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ChuyenVien = 'X' THEN 1 ELSE 0 END) AS xGroupChuyenVien, 	" +
                   "SUM(CASE WHEN #tmpBGroup.CanSu = 'X' THEN 1 ELSE 0 END) AS xGroupCanSu, 	" +
                   "SUM(CASE WHEN #tmpBGroup.NgachConLai = 'X' THEN 1 ELSE 0 END) AS xGroupNgachConLai,	" +
                   " MAX(#tmpBGroup.GhiChu) AS GhiChu " +
                   "INTO #tmpC	" +
                   "FROM #tmpBGroup	" +
                   "GROUP BY #tmpBGroup.EmployeeTypeId	" +
                   "SELECT * FROM #tmpC	 ";

            return sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="kindTypeReport"></param>
        /// <param name="employeeType"></param>
        /// <returns></returns>
        public static string GetStore_CountTotal(string departments, string kindTypeReport, string employeeType)
        {
            var sql = string.Empty;
            sql += "	IF OBJECT_ID('tempdb..#tmpB') IS NOT NULL DROP Table #tmpB 		" +
                   "	SELECT 		" +
                   "	h.EmployeeTypeId AS EmployeeTypeId,		" +
                   "   (SELECT TOP 1 emp.Name	FROM cat_EmployeeType emp WHERE emp.Id = h.EmployeeTypeId) AS TenLoaiCanBo, " +
                   "	(SELECT dd.Name FROM cat_Department dd WHERE dd.Id = h.DepartmentId ) AS 'TenDonVi', 		" +
                   "	CASE WHEN (h.CPVOfficialJoinedDate IS NOT NULL) THEN 'X' ELSE '' END AS DangVien, 		" +
                   "	CASE WHEN (SELECT TOP 1 IsMinority FROM cat_Folk WHERE Id = h.FolkId) = '1' THEN 'X' ELSE '' END AS DanTocThieuSo, 		" +
                   "	CASE WHEN h.Sex = '0' THEN 'X' ELSE '' END AS Nu, 		" +
                   "	CASE WHEN h.Sex = '1' THEN 'X' ELSE '' END AS Nam, 		" +
                   "	CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 30) THEN 'X' ELSE '' END AS Duoi30, 		" +
                   "	CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 30 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 40) THEN 'X' ELSE '' END AS Tu31Den40, 		" +
                   "	CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 40 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 50) THEN 'X' ELSE '' END AS Tu41Den50, 		" +
                   "	CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 50 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 55 AND h.Sex = '0') THEN 'X' ELSE '' END AS Nu51Den55, 		" +
                   "	CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 50 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 55 AND h.Sex = '1') THEN 'X' ELSE '' END AS Nam51Den55,		" +
                   "	CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 55 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 60 AND h.Sex = '1') THEN 'X' ELSE '' END AS Nam56Den60, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CVC' THEN 'X' ELSE '' END AS ChuyenVienChinh, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CV' THEN 'X' ELSE '' END AS ChuyenVien, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CS' THEN 'X' ELSE '' END AS CanSu, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CQDT' THEN 'X' ELSE '' END AS NgachConLai, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_BasicEducation WHERE Id = H.BasicEducationId) = 'THCS' THEN 'X' ELSE '' END AS THCS, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_BasicEducation WHERE Id = H.BasicEducationId) = 'THPT' THEN 'X' ELSE '' END AS THPT,		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'ThS' THEN 'X' ELSE '' END AS ThacSi, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'DH' THEN 'X' ELSE '' END AS DaiHoc, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'CD' THEN 'X' ELSE '' END AS CaoDang, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'TC' THEN 'X' ELSE '' END AS TrungCap, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'SC' OR (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'TS' THEN 'X' ELSE '' END AS TrinhDoChuyenMonConLai, 		" +
                   "	CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = H.PoliticLevelId) = '4' THEN 'X' ELSE '' END AS CuNhanChinhTri, 		" +
                   "	CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = H.PoliticLevelId) = '3' THEN 'X' ELSE '' END AS CaoCapChinhTri, 		" +
                   "	CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = H.PoliticLevelId) = '2' THEN 'X' ELSE '' END AS TrungCapChinhTri, 		" +
                   "	'' AS ChuongTrinhChuyenVien,		" +
                   "	'' AS QLGD,		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = H.ITLevelId) = 'DH' THEN 'X' ELSE '' END AS DaiHocTinHoc, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = H.ITLevelId) = 'CD' THEN 'X' ELSE '' END AS CaoDangTinHoc, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = H.ITLevelId) = 'TC' THEN 'X' ELSE '' END AS TrungCapTinHoc, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = H.ITLevelId) = 'CC' THEN 'X' ELSE '' END AS ChungChiTinHoc, 		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = H.LanguageLevelId) = 'DHTA' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId  WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'DHTA') = 'DHTA' THEN 'X' ELSE '' END AS DaiHocTiengAnh,		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = H.LanguageLevelId) = 'CCTA' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId  WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'CCTA') = 'CCTA' THEN 'X' ELSE '' END AS ChungChiTiengAnh,		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = H.LanguageLevelId) = 'DHNNK' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'DHNNK') = 'DHNNK' THEN 'X' ELSE '' END AS DaiHocNgoaiNguKhac,		" +
                   "	CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = H.LanguageLevelId) = 'CCNNK' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'CCNNK') = 'CCNNK' THEN 'X' ELSE '' END AS ChungChiNgoaiNguKhac, 		" +
                   "	0 AS 'xTongSo',		" +
                   "	0 AS 'xDangVien', 		" +
                   "	0 AS 'xDanTocThieuSo',		" +
                   "	0 AS 'xNu', 		" +
                   "	0 AS 'xNam', 		" +
                   "	0 AS 'xDuoi30', 		" +
                   "	0 AS 'x31Den40', 		" +
                   "	0 AS 'x41Den50', 		" +
                   "	0 AS 'xNu51Den55', 		" +
                   "	0 AS 'xNam51Den55', 		" +
                   "	0 AS 'xNam56Den60', 		" +
                   "	0 AS 'xChuyenVienChinh', 		" +
                   "	0 AS 'xChuyenVien', 		" +
                   "	0 AS 'xCanSu', 		" +
                   "	0 AS 'xNgachConLai',		" +
                   "	0 AS 'xTHCS', 		" +
                   "	0 AS 'xTHPT', 		" +
                   "	0 AS 'xThacSi', 		" +
                   "	0 AS 'xDaiHoc', 		" +
                   "	0 AS 'xCaoDang',		" +
                   "	0 AS 'xTrungCap', 		" +
                   "	0 AS 'xTrinhDoChuyenMonConLai', 		" +
                   "	0 as 'xCuNhanChinhTri', 		" +
                   "	0 as 'xCaoCapChinhTri',		" +
                   "	0 as 'xTrungCapChinhTri', 		" +
                   "	0 as 'xDaiHocTinHoc',		" +
                   "	0 as 'xCaoDangTinHoc',		" +
                   "	0 as 'xTrungCapTinHoc',		" +
                   "	0 as 'xChungChiTinHoc', 		" +
                   "	0 as 'xDaiHocTiengAnh', 		" +
                   "	0 as 'xChungChiTiengAnh', 		" +
                   "	0 as 'xDaiHocNgoaiNguKhac', 		" +
                   "	0 as 'xChungChiNgoaiNguKhac'		" +
                   "	INTO #tmpB 		" +
                   "	FROM hr_Record h " +
                   "   LEFT JOIN cat_EmployeeType ce ON ce.Id = h.EmployeeTypeId " +
                   "   WHERE h.DepartmentId IN ({0}) ".FormatWith(departments);
            if("LaNu" == kindTypeReport)
            {
                sql += "AND h.Sex = '0'  ";
            }
            if(!string.IsNullOrEmpty(employeeType))
            {
                sql += " AND ce.[Group] = '{0}' ".FormatWith(employeeType);
            }
            sql += "	ORDER BY h.EmployeeTypeId 		" +
                   "	UPDATE	#tmpB 	" +
                   "	SET 		" +
                   "	xTongSo = xwTongSo, 		" +
                   "	xDangVien = xwDangVien, 		" +
                   "	xDanTocThieuSo = xwDanTocThieuSo, 		" +
                   "	xNu = xwNu, 		" +
                   "	xNam = xwNam, 		" +
                   "	xDuoi30 = xwDuoi30, 		" +
                   "	x31Den40 = xw31Den40, 		" +
                   "	x41Den50 = xw41Den50, 		" +
                   "	xNu51Den55 = xwNu51Den55, 		" +
                   "	xNam51Den55 = xwNam51Den55, 		" +
                   "	xNam56Den60 = xwNam56Den60, 		" +
                   "	xThacSi = xwThacSi, 		" +
                   "	xDaiHoc = xwDaiHoc, 		" +
                   "	xCaoDang = xwCaoDang, 		" +
                   "	xTrungCap = xwTrungCap, 		" +
                   "	xTrinhDoChuyenMonConLai = xwTrinhDoChuyenMonConLai, 		" +
                   "	xTHCS = xwTHCS, 		" +
                   "	xTHPT = xwTHPT, 		" +
                   "	xCuNhanChinhTri = xwCuNhanChinhTri,		" +
                   "	xCaoCapChinhTri = xwCaoCapChinhTri, 		" +
                   "	xTrungCapChinhTri = xwTrungCapChinhTri, 		" +
                   "	xDaiHocTinHoc = xwDaiHocTinHoc, 		" +
                   "	xCaoDangTinHoc = xwCaoDangTinHoc, 		" +
                   "	xTrungCapTinHoc = xwTrungCapTinHoc, 		" +
                   "	xChungChiTinHoc = xwChungChiTinHoc, 		" +
                   "	xDaiHocTiengAnh = xwDaiHocTiengAnh, 		" +
                   "	xChungChiTiengAnh = xwChungChiTiengAnh, 		" +
                   "	xDaiHocNgoaiNguKhac = xwDaiHocNgoaiNguKhac, 		" +
                   "	xChungChiNgoaiNguKhac = xwChungChiNgoaiNguKhac, 		" +
                   "	xChuyenVienChinh = xwChuyenVienChinh, 		" +
                   "	xChuyenVien = xwChuyenVien, 		" +
                   "	xCanSu = xwCanSu, 		" +
                   "	xNgachConLai = xwNgachConLai		" +
                   "	FROM #tmpB 		" +
                   "	INNER JOIN (SELECT 		" +
                   "	SUM(1) AS xwTongSo, 		" +
                   "	SUM(CASE WHEN #tmpB.DangVien = 'X' THEN 1 ELSE 0 END) AS xwDangVien, 		" +
                   "	SUM(CASE WHEN #tmpB.DanTocThieuSo = 'X' THEN 1 ELSE 0 END) AS xwDanTocThieuSo, 		" +
                   "	SUM(CASE WHEN #tmpB.Nu = 'X' THEN 1 ELSE 0 END) AS xwNu, 		" +
                   "	SUM(CASE WHEN #tmpB.Nam = 'X' THEN 1 ELSE 0 END) AS xwNam, 		" +
                   "	SUM(CASE WHEN #tmpB.Duoi30 = 'X' THEN 1 ELSE 0 END) AS xwDuoi30, 		" +
                   "	SUM(CASE WHEN #tmpB.Tu31Den40 = 'X' THEN 1 ELSE 0 END) AS xw31Den40, 		" +
                   "	SUM(CASE WHEN #tmpB.Tu41Den50 = 'X' THEN 1 ELSE 0 END) AS xw41Den50, 		" +
                   "	SUM(CASE WHEN #tmpB.Nu51Den55 = 'X' THEN 1 ELSE 0 END) AS xwNu51Den55, 		" +
                   "	SUM(CASE WHEN #tmpB.Nam51Den55 = 'X' THEN 1 ELSE 0 END) AS xwNam51Den55, 		" +
                   "	SUM(CASE WHEN #tmpB.Nam56Den60 = 'X' THEN 1 ELSE 0 END) AS xwNam56Den60, 		" +
                   "	SUM(CASE WHEN #tmpB.ThacSi = 'X' THEN 1 ELSE 0 END) AS xwThacSi, 		" +
                   "	SUM(CASE WHEN #tmpB.DaiHoc = 'X' THEN 1 ELSE 0 END) AS xwDaiHoc, 		" +
                   "	SUM(CASE WHEN #tmpB.CaoDang = 'X' THEN 1 ELSE 0 END) AS xwCaoDang, 		" +
                   "	SUM(CASE WHEN #tmpB.TrungCap = 'X' THEN 1 ELSE 0 END) AS xwTrungCap, 		" +
                   "	SUM(CASE WHEN #tmpB.TrinhDoChuyenMonConLai = 'X' THEN 1 ELSE 0 END) AS xwTrinhDoChuyenMonConLai, 		" +
                   "	SUM(CASE WHEN #tmpB.THCS = 'X' THEN 1 ELSE 0 END) AS xwTHCS, 		" +
                   "	SUM(CASE WHEN #tmpB.THPT = 'X' THEN 1 ELSE 0 END) AS xwTHPT, 		" +
                   "	SUM(CASE WHEN #tmpB.CuNhanChinhTri = 'X' THEN 1 ELSE 0 END) AS xwCuNhanChinhTri, 		" +
                   "	SUM(CASE WHEN #tmpB.CaoCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xwCaoCapChinhTri, 		" +
                   "	SUM(CASE WHEN #tmpB.TrungCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xwTrungCapChinhTri, 		" +
                   "	SUM(CASE WHEN #tmpB.DaiHocTinHoc = 'X' THEN 1 ELSE 0 END) AS xwDaiHocTinHoc, 		" +
                   "	SUM(CASE WHEN #tmpB.CaoDangTinHoc = 'X' THEN 1 ELSE 0 END) AS xwCaoDangTinHoc, 		" +
                   "	SUM(CASE WHEN #tmpB.TrungCapTinHoc = 'X' THEN 1 ELSE 0 END) AS xwTrungCapTinHoc, 		" +
                   "	SUM(CASE WHEN #tmpB.ChungChiTinHoc = 'X' THEN 1 ELSE 0 END) AS xwChungChiTinHoc, 		" +
                   "	SUM(CASE WHEN #tmpB.DaiHocTiengAnh = 'X' THEN 1 ELSE 0 END) AS xwDaiHocTiengAnh, 		" +
                   "	SUM(CASE WHEN #tmpB.ChungChiTiengAnh = 'X' THEN 1 ELSE 0 END) AS xwChungChiTiengAnh, 		" +
                   "	SUM(CASE WHEN #tmpB.DaiHocNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xwDaiHocNgoaiNguKhac, 		" +
                   "	SUM(CASE WHEN #tmpB.ChungChiNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xwChungChiNgoaiNguKhac, 		" +
                   "	SUM(CASE WHEN #tmpB.ChuyenVienChinh = 'X' THEN 1 ELSE 0 END) AS  xwChuyenVienChinh, 		" +
                   "	SUM(CASE WHEN #tmpB.ChuyenVien = 'X' THEN 1 ELSE 0 END) AS xwChuyenVien, 		" +
                   "	SUM(CASE WHEN #tmpB.CanSu = 'X' THEN 1 ELSE 0 END) AS xwCanSu, 		" +
                   "	SUM(CASE WHEN #tmpB.NgachConLai = 'X' THEN 1 ELSE 0 END) AS xwNgachConLai		" +
                   "	FROM #tmpB	" +
                   "	) AS TMP ON 1=1 	" +
                   "	SELECT * FROM #tmpB " +
                   " ORDER BY EmployeeTypeId";

            return sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="employeeType"></param>
        /// <returns></returns>
        public static string GetStore_QuantityEthnicMinorityCountGroupTotal(string departments, string employeeType)
        {
            var sql = string.Empty;
            sql += " IF OBJECT_ID('tempdb..#tmpBGroup') IS NOT NULL DROP Table #tmpBGroup 		" +
                   "SELECT 		" +
                   "h.EmployeeTypeId AS EmployeeTypeId,		" +
                   "(SELECT TOP 1 emp.Name	FROM cat_EmployeeType emp WHERE emp.Id = h.EmployeeTypeId) AS TenLoaiCanBo,	" +
                   "(SELECT dd.Name FROM cat_Department dd WHERE dd.Id = h.DepartmentId ) AS 'TenDonVi', 		" +
                   "CASE WHEN (h.CPVOfficialJoinedDate IS NOT NULL) THEN 'X' ELSE '' END AS DangVien, 		" +
                   " CASE WHEN cf.Name = N'Thái' THEN 'X' ELSE '' END AS DanTocThai,	" +
                   " CASE WHEN cf.Name = N'Mông' THEN 'X' ELSE '' END AS DanTocMong,	" +
                   " CASE WHEN cf.Name = N'Hà Nhì' THEN 'X' ELSE '' END AS DanTocHaNhi,	" +
                   " CASE WHEN cf.Name = N'Tày' THEN 'X' ELSE '' END AS DanTocTay,	" +
                   " CASE WHEN cf.Name = N'Mường' THEN 'X' ELSE '' END AS DanTocMuong,	" +
                   " CASE WHEN cf.Name = N'Dao' THEN 'X' ELSE '' END AS DanTocDao,	" +
                   " CASE WHEN cf.Name = N'Giáy' THEN 'X' ELSE '' END AS DanTocGiay,	" +
                   " CASE WHEN cf.Name = N'Cống' THEN 'X' ELSE '' END AS DanTocCong,	" +
                   " CASE WHEN cf.Name = N'Hoa' THEN 'X' ELSE '' END AS DanTocHoa,	" +
                   " CASE WHEN cf.Name = N'Si La' THEN 'X' ELSE '' END AS DanTocSiLa,	" +
                   " CASE WHEN cf.Name = N'Nùng' THEN 'X' ELSE '' END AS DanTocNung,	" +
                   " CASE WHEN cf.Name = N'Cao Lan' THEN 'X' ELSE '' END AS DanTocCaoLan,	" +
                   " CASE WHEN cf.Name = N'La Hủ' THEN 'X' ELSE '' END AS DanTocLaHu,	" +
                   " CASE WHEN cf.Name = N'Thổ' THEN 'X' ELSE '' END AS DanTocTho,	" +
                   " CASE WHEN cf.Name not in( N'Giáy', N'Mông', N'Hà Nhì',N'Tày', N'Mường', N'Dao' , N'Giáy', N'Cống', N'Hoa', N'Si La',N'Nùng',N'Cao Lan',N'La Hủ', N'Thổ' ) THEN 'X' ELSE '' END AS DanTocKhac, " +
                   "CASE WHEN h.Sex = '0' THEN 'X' ELSE '' END AS Nu, 		" +
                   "CASE WHEN h.Sex = '1' THEN 'X' ELSE '' END AS Nam,	" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 30) THEN 'X' ELSE '' END AS Duoi30, 		" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 30 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 40) THEN 'X' ELSE '' END AS Tu31Den40, 		" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 40 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 50) THEN 'X' ELSE '' END AS Tu41Den50, 		" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 50 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 55 AND h.Sex = '0') THEN 'X' ELSE '' END AS Nu51Den55, 		" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 50 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 55 AND h.Sex = '1') THEN 'X' ELSE '' END AS Nam51Den55,		" +
                   "CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 55 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 60 AND h.Sex = '1') THEN 'X' ELSE '' END AS Nam56Den60, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CVC' THEN 'X' ELSE '' END AS ChuyenVienChinh, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CV' THEN 'X' ELSE '' END AS ChuyenVien, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CS' THEN 'X' ELSE '' END AS CanSu, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CQDT' THEN 'X' ELSE '' END AS NgachConLai, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_BasicEducation WHERE Id = h.BasicEducationId) = 'THCS' THEN 'X' ELSE '' END AS THCS, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_BasicEducation WHERE Id = h.BasicEducationId) = 'THPT' THEN 'X' ELSE '' END AS THPT,		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = h.EducationId) = 'ThS' THEN 'X' ELSE '' END AS ThacSi, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = h.EducationId) = 'DH' THEN 'X' ELSE '' END AS DaiHoc, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = h.EducationId) = 'CD' THEN 'X' ELSE '' END AS CaoDang, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = h.EducationId) = 'TC' THEN 'X' ELSE '' END AS TrungCap, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = h.EducationId) = 'SC' OR (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'TS' THEN 'X' ELSE '' END AS TrinhDoChuyenMonConLai, 		" +
                   "CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = h.PoliticLevelId) = '4' THEN 'X' ELSE '' END AS CuNhanChinhTri, 		" +
                   "CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = h.PoliticLevelId) = '3' THEN 'X' ELSE '' END AS CaoCapChinhTri, 		" +
                   "CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = h.PoliticLevelId) = '2' THEN 'X' ELSE '' END AS TrungCapChinhTri, 		" +
                   "'' AS ChuongTrinhChuyenVien,		" +
                   "'' AS QLGD,		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = h.ITLevelId) = 'DH' THEN 'X' ELSE '' END AS DaiHocTinHoc, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = h.ITLevelId) = 'CD' THEN 'X' ELSE '' END AS CaoDangTinHoc, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = h.ITLevelId) = 'TC' THEN 'X' ELSE '' END AS TrungCapTinHoc, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = h.ITLevelId) = 'CC' THEN 'X' ELSE '' END AS ChungChiTinHoc, 		" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = h.LanguageLevelId) = 'DHTA' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId  WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'DHTA') = 'DHTA' THEN 'X' ELSE '' END AS DaiHocTiengAnh,	" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = h.LanguageLevelId) = 'CCTA' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId  WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'CCTA') = 'CCTA' THEN 'X' ELSE '' END AS ChungChiTiengAnh,	" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = h.LanguageLevelId) = 'DHNNK' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'DHNNK') = 'DHNNK' THEN 'X' ELSE '' END AS DaiHocNgoaiNguKhac,	" +
                   "CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = h.LanguageLevelId) = 'CCNNK' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'CCNNK') = 'CCNNK' THEN 'X' ELSE '' END AS ChungChiNgoaiNguKhac, 	" +
                   " '' AS GhiChu " +
                   "INTO #tmpBGroup 		" +
                   "FROM hr_Record h 		" +
                   "   LEFT JOIN cat_Folk cf on cf.Id = h.FolkId " +
                   "   LEFT JOIN cat_EmployeeType ce ON ce.Id = h.EmployeeTypeId " +
                   "WHERE h.DepartmentId IN ({0}) ".FormatWith(departments) +
                   "   AND cf.IsMinority = '1' ";
            if(!string.IsNullOrEmpty(employeeType))
            {
                sql += " AND ce.[Group] = '{0}' ".FormatWith(employeeType);
            }
            sql += "ORDER BY h.EmployeeTypeId 		" +
                   "SELECT  " +
                   "MAX(#tmpBGroup.TenLoaiCanBo) AS TenLoaiCanBo, " +
                   "SUM(CASE WHEN #tmpBGroup.DangVien = 'X' THEN 1 ELSE 0 END) AS xGroupDangVien,	" +
                   " SUM(CASE WHEN #tmpBGroup.DanTocThai = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocThai , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocMong = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocMong , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocHaNhi = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocHaNhi , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocTay = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocTay , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocMuong = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocMuong , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocDao = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocDao , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocGiay = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocGiay , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocCong = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocCong , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocHoa = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocHoa , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocSiLa = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocSiLa , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocNung = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocNung , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocCaoLan = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocCaoLan , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocLaHu = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocLaHu , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocTho = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocTho , " +
                   " SUM(CASE WHEN #tmpBGroup.DanTocKhac = 'X' THEN 1 ELSE 0 END) AS xGroupDanTocKhac , " +
                   "SUM(CASE WHEN #tmpBGroup.Nu = 'X' THEN 1 ELSE 0 END) AS xGroupNu,	" +
                   "SUM(CASE WHEN #tmpBGroup.Nam = 'X' THEN 1 ELSE 0 END) AS xGroupNam, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Duoi30 = 'X' THEN 1 ELSE 0 END) AS xGroupDuoi30, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Tu31Den40 = 'X' THEN 1 ELSE 0 END) AS xGroup31Den40, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Tu41Den50 = 'X' THEN 1 ELSE 0 END) AS xGroup41Den50, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Nu51Den55 = 'X' THEN 1 ELSE 0 END) AS xGroupNu51Den55, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Nam51Den55 = 'X' THEN 1 ELSE 0 END) AS xGroupNam51Den55, 	" +
                   "SUM(CASE WHEN #tmpBGroup.Nam56Den60 = 'X' THEN 1 ELSE 0 END) AS xGroupNam56Den60, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ThacSi = 'X' THEN 1 ELSE 0 END) AS xGroupThacSi, 	" +
                   "SUM(CASE WHEN #tmpBGroup.DaiHoc = 'X' THEN 1 ELSE 0 END) AS xGroupDaiHoc, 	" +
                   "SUM(CASE WHEN #tmpBGroup.CaoDang = 'X' THEN 1 ELSE 0 END) AS xGroupCaoDang, 	" +
                   "SUM(CASE WHEN #tmpBGroup.TrungCap = 'X' THEN 1 ELSE 0 END) AS xGroupTrungCap, 	" +
                   "SUM(CASE WHEN #tmpBGroup.TrinhDoChuyenMonConLai = 'X' THEN 1 ELSE 0 END) AS xGroupTrinhDoChuyenMonConLai, 	" +
                   "SUM(CASE WHEN #tmpBGroup.THCS = 'X' THEN 1 ELSE 0 END) AS xGroupTHCS, 	" +
                   "SUM(CASE WHEN #tmpBGroup.THPT = 'X' THEN 1 ELSE 0 END) AS xGroupTHPT, 	" +
                   "SUM(CASE WHEN #tmpBGroup.CuNhanChinhTri = 'X' THEN 1 ELSE 0 END) AS xGroupCuNhanChinhTri, 	" +
                   "SUM(CASE WHEN #tmpBGroup.CaoCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xGroupCaoCapChinhTri, 	" +
                   "SUM(CASE WHEN #tmpBGroup.TrungCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xGroupTrungCapChinhTri, 	" +
                   "SUM(CASE WHEN #tmpBGroup.DaiHocTinHoc = 'X' THEN 1 ELSE 0 END) AS xGroupDaiHocTinHoc, 	" +
                   "SUM(CASE WHEN #tmpBGroup.CaoDangTinHoc = 'X' THEN 1 ELSE 0 END) AS xGroupCaoDangTinHoc, 	" +
                   "SUM(CASE WHEN #tmpBGroup.TrungCapTinHoc = 'X' THEN 1 ELSE 0 END) AS xGroupTrungCapTinHoc, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ChungChiTinHoc = 'X' THEN 1 ELSE 0 END) AS xGroupChungChiTinHoc, 	" +
                   "SUM(CASE WHEN #tmpBGroup.DaiHocTiengAnh = 'X' THEN 1 ELSE 0 END) AS xGroupDaiHocTiengAnh, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ChungChiTiengAnh = 'X' THEN 1 ELSE 0 END) AS xGroupChungChiTiengAnh, 	" +
                   "SUM(CASE WHEN #tmpBGroup.DaiHocNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xGroupDaiHocNgoaiNguKhac, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ChungChiNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xGroupChungChiNgoaiNguKhac, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ChuyenVienChinh = 'X' THEN 1 ELSE 0 END) AS  xGroupChuyenVienChinh, 	" +
                   "SUM(CASE WHEN #tmpBGroup.ChuyenVien = 'X' THEN 1 ELSE 0 END) AS xGroupChuyenVien, 	" +
                   "SUM(CASE WHEN #tmpBGroup.CanSu = 'X' THEN 1 ELSE 0 END) AS xGroupCanSu, 	" +
                   "SUM(CASE WHEN #tmpBGroup.NgachConLai = 'X' THEN 1 ELSE 0 END) AS xGroupNgachConLai,	" +
                   " MAX(#tmpBGroup.GhiChu) AS GhiChu " +
                   "INTO #tmpC	" +
                   "FROM #tmpBGroup	" +
                   "GROUP BY #tmpBGroup.EmployeeTypeId	" +
                   "SELECT * FROM #tmpC	 ";

            return sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="employeeType"></param>
        /// <returns></returns>
        public static string GetStore_QuantityEthnicMinorityCountTotal(string departments, string employeeType)
        {
            var sql = string.Empty;
            sql += "	IF OBJECT_ID('tempdb..#tmpB') IS NOT NULL DROP Table #tmpB 		" +
                   " SELECT 		" +
                   " h.EmployeeTypeId AS EmployeeTypeId,		" +
                   "   (SELECT TOP 1 emp.Name	FROM cat_EmployeeType emp WHERE emp.Id = h.EmployeeTypeId) AS TenLoaiCanBo, " +
                   " (SELECT dd.Name FROM cat_Department dd WHERE dd.Id = h.DepartmentId ) AS 'TenDonVi', 		" +
                   " CASE WHEN (h.CPVOfficialJoinedDate IS NOT NULL) THEN 'X' ELSE '' END AS DangVien, 		" +
                   " CASE WHEN cf.Name = N'Thái' THEN 'X' ELSE '' END AS DanTocThai,	" +
                   " CASE WHEN cf.Name = N'Mông' THEN 'X' ELSE '' END AS DanTocMong,	" +
                   " CASE WHEN cf.Name = N'Hà Nhì' THEN 'X' ELSE '' END AS DanTocHaNhi,	" +
                   " CASE WHEN cf.Name = N'Tày' THEN 'X' ELSE '' END AS DanTocTay,	" +
                   " CASE WHEN cf.Name = N'Mường' THEN 'X' ELSE '' END AS DanTocMuong,	" +
                   " CASE WHEN cf.Name = N'Dao' THEN 'X' ELSE '' END AS DanTocDao,	" +
                   " CASE WHEN cf.Name = N'Giáy' THEN 'X' ELSE '' END AS DanTocGiay,	" +
                   " CASE WHEN cf.Name = N'Cống' THEN 'X' ELSE '' END AS DanTocCong,	" +
                   " CASE WHEN cf.Name = N'Hoa' THEN 'X' ELSE '' END AS DanTocHoa,	" +
                   " CASE WHEN cf.Name = N'Si La' THEN 'X' ELSE '' END AS DanTocSiLa,	" +
                   " CASE WHEN cf.Name = N'Nùng' THEN 'X' ELSE '' END AS DanTocNung,	" +
                   " CASE WHEN cf.Name = N'Cao Lan' THEN 'X' ELSE '' END AS DanTocCaoLan,	" +
                   " CASE WHEN cf.Name = N'La Hủ' THEN 'X' ELSE '' END AS DanTocLaHu,	" +
                   " CASE WHEN cf.Name = N'Thổ' THEN 'X' ELSE '' END AS DanTocTho,	" +
                   " CASE WHEN cf.Name not in( N'Giáy', N'Mông', N'Hà Nhì',N'Tày', N'Mường', N'Dao' , N'Giáy', N'Cống', N'Hoa', N'Si La',N'Nùng',N'Cao Lan',N'La Hủ', N'Thổ' ) THEN 'X' ELSE '' END AS DanTocKhac, " +
                   " CASE WHEN h.Sex = '0' THEN 'X' ELSE '' END AS Nu, 		" +
                   " CASE WHEN h.Sex = '1' THEN 'X' ELSE '' END AS Nam, 		" +
                   " CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 30) THEN 'X' ELSE '' END AS Duoi30, 		" +
                   " CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 30 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 40) THEN 'X' ELSE '' END AS Tu31Den40, 		" +
                   " CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 40 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 50) THEN 'X' ELSE '' END AS Tu41Den50, 		" +
                   " CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 50 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 55 AND h.Sex = '0') THEN 'X' ELSE '' END AS Nu51Den55, 		" +
                   " CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 50 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 55 AND h.Sex = '1') THEN 'X' ELSE '' END AS Nam51Den55,		" +
                   " CASE WHEN (h.BirthDate IS NOT NULL AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) > 55 AND DATEDIFF(YEAR, h.BirthDate, GETDATE()) <= 60 AND h.Sex = '1') THEN 'X' ELSE '' END AS Nam56Den60, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CVC' THEN 'X' ELSE '' END AS ChuyenVienChinh, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CV' THEN 'X' ELSE '' END AS ChuyenVien, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CS' THEN 'X' ELSE '' END AS CanSu, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_ManagementLevel ml WHERE ml.Id = h.ManagementLevelId) = 'CQDT' THEN 'X' ELSE '' END AS NgachConLai, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_BasicEducation WHERE Id = H.BasicEducationId) = 'THCS' THEN 'X' ELSE '' END AS THCS, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_BasicEducation WHERE Id = H.BasicEducationId) = 'THPT' THEN 'X' ELSE '' END AS THPT,		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'ThS' THEN 'X' ELSE '' END AS ThacSi, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'DH' THEN 'X' ELSE '' END AS DaiHoc, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'CD' THEN 'X' ELSE '' END AS CaoDang, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'TC' THEN 'X' ELSE '' END AS TrungCap, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'SC' OR (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = H.EducationId) = 'TS' THEN 'X' ELSE '' END AS TrinhDoChuyenMonConLai, 		" +
                   " CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = H.PoliticLevelId) = '4' THEN 'X' ELSE '' END AS CuNhanChinhTri, 		" +
                   " CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = H.PoliticLevelId) = '3' THEN 'X' ELSE '' END AS CaoCapChinhTri, 		" +
                   " CASE WHEN (SELECT TOP 1 Id FROM cat_PoliticLevel pl WHERE pl.Id = H.PoliticLevelId) = '2' THEN 'X' ELSE '' END AS TrungCapChinhTri, 		" +
                   " '' AS ChuongTrinhChuyenVien,		" +
                   " '' AS QLGD,		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = H.ITLevelId) = 'DH' THEN 'X' ELSE '' END AS DaiHocTinHoc, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = H.ITLevelId) = 'CD' THEN 'X' ELSE '' END AS CaoDangTinHoc, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = H.ITLevelId) = 'TC' THEN 'X' ELSE '' END AS TrungCapTinHoc, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_ITLevel itl WHERE itl.Id = H.ITLevelId) = 'CC' THEN 'X' ELSE '' END AS ChungChiTinHoc, 		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = H.LanguageLevelId) = 'DHTA' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId  WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'DHTA') = 'DHTA' THEN 'X' ELSE '' END AS DaiHocTiengAnh,		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = H.LanguageLevelId) = 'CCTA' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId  WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'CCTA') = 'CCTA' THEN 'X' ELSE '' END AS ChungChiTiengAnh,		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = H.LanguageLevelId) = 'DHNNK' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'DHNNK') = 'DHNNK' THEN 'X' ELSE '' END AS DaiHocNgoaiNguKhac,		" +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_LanguageLevel ll WHERE ll.Id = H.LanguageLevelId) = 'CCNNK' OR (SELECT DN.[Group] FROM cat_LanguageLevel DN LEFT JOIN hr_Language HN ON H.Id = HN.RecordId WHERE DN.Id = HN.LanguageId AND DN.[Group] = 'CCNNK') = 'CCNNK' THEN 'X' ELSE '' END AS ChungChiNgoaiNguKhac, 		" +
                   " 0 AS 'xTongSo',		" +
                   " 0 AS 'xDangVien', 		" +
                   " 0 AS 'xDanTocThai',  " +
                   " 0 AS 'xDanTocMong',  " +
                   " 0 AS 'xDanTocHaNhi',  " +
                   " 0 AS 'xDanTocTay',  " +
                   " 0 AS 'xDanTocMuong',  " +
                   " 0 AS 'xDanTocDao',  " +
                   " 0 AS 'xDanTocGiay',  " +
                   " 0 AS 'xDanTocCong',  " +
                   " 0 AS 'xDanTocHoa',  " +
                   " 0 AS 'xDanTocSiLa',  " +
                   " 0 AS 'xDanTocNung',  " +
                   " 0 AS 'xDanTocCaoLan',  " +
                   " 0 AS 'xDanTocLaHu',  " +
                   " 0 AS 'xDanTocTho',  " +
                   " 0 AS 'xDanTocKhac',  " +
                   " 0 AS 'xNu', 		" +
                   " 0 AS 'xNam', 		" +
                   " 0 AS 'xDuoi30', 		" +
                   " 0 AS 'x31Den40', 		" +
                   " 0 AS 'x41Den50', 		" +
                   " 0 AS 'xNu51Den55', 		" +
                   " 0 AS 'xNam51Den55', 		" +
                   " 0 AS 'xNam56Den60', 		" +
                   " 0 AS 'xChuyenVienChinh', 		" +
                   " 0 AS 'xChuyenVien', 		" +
                   " 0 AS 'xCanSu', 		" +
                   " 0 AS 'xNgachConLai',		" +
                   " 0 AS 'xTHCS', 		" +
                   " 0 AS 'xTHPT', 		" +
                   " 0 AS 'xThacSi', 		" +
                   " 0 AS 'xDaiHoc', 		" +
                   " 0 AS 'xCaoDang',		" +
                   " 0 AS 'xTrungCap', 		" +
                   " 0 AS 'xTrinhDoChuyenMonConLai', 		" +
                   " 0 as 'xCuNhanChinhTri', 		" +
                   " 0 as 'xCaoCapChinhTri',		" +
                   " 0 as 'xTrungCapChinhTri', 		" +
                   " 0 as 'xDaiHocTinHoc',		" +
                   " 0 as 'xCaoDangTinHoc',		" +
                   " 0 as 'xTrungCapTinHoc',		" +
                   " 0 as 'xChungChiTinHoc', 		" +
                   " 0 as 'xDaiHocTiengAnh', 		" +
                   " 0 as 'xChungChiTiengAnh', 		" +
                   " 0 as 'xDaiHocNgoaiNguKhac', 		" +
                   " 0 as 'xChungChiNgoaiNguKhac'		" +
                   " INTO #tmpB 		" +
                   " FROM hr_Record h " +
                   "   LEFT JOIN cat_Folk cf on cf.Id = h.FolkId " +
                   "   LEFT JOIN cat_EmployeeType ce ON ce.Id = h.EmployeeTypeId " +
                   "   WHERE h.DepartmentId IN ({0}) ".FormatWith(departments) +
                   "   AND cf.IsMinority = '1' ";
            if(!string.IsNullOrEmpty(employeeType))
            {
                sql += " AND ce.[Group] = '{0}' ".FormatWith(employeeType);
            }
            sql += " ORDER BY h.EmployeeTypeId 		" +
                   " UPDATE	#tmpB 	" +
                   " SET 		" +
                   " xTongSo = xwTongSo, 		" +
                   " xDangVien = xwDangVien, 		" +
                   " xDanTocThai = xwDanTocThai	, " +
                   " xDanTocMong = xwDanTocMong	, " +
                   " xDanTocHaNhi = xwDanTocHaNhi	, " +
                   " xDanTocTay = xwDanTocTay	, " +
                   " xDanTocMuong = xwDanTocMuong	, " +
                   " xDanTocDao = xwDanTocDao	, " +
                   " xDanTocGiay = xwDanTocGiay	, " +
                   " xDanTocCong = xwDanTocCong	, " +
                   " xDanTocHoa = xwDanTocHoa	, " +
                   " xDanTocSiLa = xwDanTocSiLa	, " +
                   " xDanTocNung = xwDanTocNung	, " +
                   " xDanTocCaoLan = xwDanTocCaoLan	, " +
                   " xDanTocLaHu = xwDanTocLaHu	, " +
                   " xDanTocTho = xwDanTocTho	, " +
                   " xDanTocKhac = xwDanTocKhac	, " +
                   " xNu = xwNu, 		" +
                   " xNam = xwNam, 		" +
                   " xDuoi30 = xwDuoi30, 		" +
                   " x31Den40 = xw31Den40, 		" +
                   " x41Den50 = xw41Den50, 		" +
                   " xNu51Den55 = xwNu51Den55, 		" +
                   " xNam51Den55 = xwNam51Den55, 		" +
                   " xNam56Den60 = xwNam56Den60, 		" +
                   " xThacSi = xwThacSi, 		" +
                   " xDaiHoc = xwDaiHoc, 		" +
                   " xCaoDang = xwCaoDang, 		" +
                   " xTrungCap = xwTrungCap, 		" +
                   " xTrinhDoChuyenMonConLai = xwTrinhDoChuyenMonConLai, 		" +
                   " xTHCS = xwTHCS, 		" +
                   " xTHPT = xwTHPT, 		" +
                   " xCuNhanChinhTri = xwCuNhanChinhTri,		" +
                   " xCaoCapChinhTri = xwCaoCapChinhTri, 		" +
                   " xTrungCapChinhTri = xwTrungCapChinhTri, 		" +
                   " xDaiHocTinHoc = xwDaiHocTinHoc, 		" +
                   " xCaoDangTinHoc = xwCaoDangTinHoc, 		" +
                   " xTrungCapTinHoc = xwTrungCapTinHoc, 		" +
                   " xChungChiTinHoc = xwChungChiTinHoc, 		" +
                   " xDaiHocTiengAnh = xwDaiHocTiengAnh, 		" +
                   " xChungChiTiengAnh = xwChungChiTiengAnh, 		" +
                   " xDaiHocNgoaiNguKhac = xwDaiHocNgoaiNguKhac, 		" +
                   " xChungChiNgoaiNguKhac = xwChungChiNgoaiNguKhac, 		" +
                   " xChuyenVienChinh = xwChuyenVienChinh, 		" +
                   " xChuyenVien = xwChuyenVien, 		" +
                   " xCanSu = xwCanSu, 		" +
                   " xNgachConLai = xwNgachConLai		" +
                   " FROM #tmpB 		" +
                   " INNER JOIN (SELECT 		" +
                   " SUM(1) AS xwTongSo, 		" +
                   " SUM(CASE WHEN #tmpB.DangVien = 'X' THEN 1 ELSE 0 END) AS xwDangVien, 		" +
                   " SUM(CASE WHEN #tmpB.DanTocThai = 'X' THEN 1 ELSE 0 END) AS xwDanTocThai , " +
                   " SUM(CASE WHEN #tmpB.DanTocMong = 'X' THEN 1 ELSE 0 END) AS xwDanTocMong , " +
                   " SUM(CASE WHEN #tmpB.DanTocHaNhi = 'X' THEN 1 ELSE 0 END) AS xwDanTocHaNhi , " +
                   " SUM(CASE WHEN #tmpB.DanTocTay = 'X' THEN 1 ELSE 0 END) AS xwDanTocTay , " +
                   " SUM(CASE WHEN #tmpB.DanTocMuong = 'X' THEN 1 ELSE 0 END) AS xwDanTocMuong , " +
                   " SUM(CASE WHEN #tmpB.DanTocDao = 'X' THEN 1 ELSE 0 END) AS xwDanTocDao , " +
                   " SUM(CASE WHEN #tmpB.DanTocGiay = 'X' THEN 1 ELSE 0 END) AS xwDanTocGiay , " +
                   " SUM(CASE WHEN #tmpB.DanTocCong = 'X' THEN 1 ELSE 0 END) AS xwDanTocCong , " +
                   " SUM(CASE WHEN #tmpB.DanTocHoa = 'X' THEN 1 ELSE 0 END) AS xwDanTocHoa , " +
                   " SUM(CASE WHEN #tmpB.DanTocSiLa = 'X' THEN 1 ELSE 0 END) AS xwDanTocSiLa , " +
                   " SUM(CASE WHEN #tmpB.DanTocNung = 'X' THEN 1 ELSE 0 END) AS xwDanTocNung , " +
                   " SUM(CASE WHEN #tmpB.DanTocCaoLan = 'X' THEN 1 ELSE 0 END) AS xwDanTocCaoLan , " +
                   " SUM(CASE WHEN #tmpB.DanTocLaHu = 'X' THEN 1 ELSE 0 END) AS xwDanTocLaHu , " +
                   " SUM(CASE WHEN #tmpB.DanTocTho = 'X' THEN 1 ELSE 0 END) AS xwDanTocTho , " +
                   " SUM(CASE WHEN #tmpB.DanTocKhac = 'X' THEN 1 ELSE 0 END) AS xwDanTocKhac , " +
                   " SUM(CASE WHEN #tmpB.Nu = 'X' THEN 1 ELSE 0 END) AS xwNu, 		" +
                   " SUM(CASE WHEN #tmpB.Nam = 'X' THEN 1 ELSE 0 END) AS xwNam, 		" +
                   " SUM(CASE WHEN #tmpB.Duoi30 = 'X' THEN 1 ELSE 0 END) AS xwDuoi30, 		" +
                   " SUM(CASE WHEN #tmpB.Tu31Den40 = 'X' THEN 1 ELSE 0 END) AS xw31Den40, 		" +
                   " SUM(CASE WHEN #tmpB.Tu41Den50 = 'X' THEN 1 ELSE 0 END) AS xw41Den50, 		" +
                   " SUM(CASE WHEN #tmpB.Nu51Den55 = 'X' THEN 1 ELSE 0 END) AS xwNu51Den55, 		" +
                   " SUM(CASE WHEN #tmpB.Nam51Den55 = 'X' THEN 1 ELSE 0 END) AS xwNam51Den55, 		" +
                   " SUM(CASE WHEN #tmpB.Nam56Den60 = 'X' THEN 1 ELSE 0 END) AS xwNam56Den60, 		" +
                   " SUM(CASE WHEN #tmpB.ThacSi = 'X' THEN 1 ELSE 0 END) AS xwThacSi, 		" +
                   " SUM(CASE WHEN #tmpB.DaiHoc = 'X' THEN 1 ELSE 0 END) AS xwDaiHoc, 		" +
                   " SUM(CASE WHEN #tmpB.CaoDang = 'X' THEN 1 ELSE 0 END) AS xwCaoDang, 		" +
                   " SUM(CASE WHEN #tmpB.TrungCap = 'X' THEN 1 ELSE 0 END) AS xwTrungCap, 		" +
                   " SUM(CASE WHEN #tmpB.TrinhDoChuyenMonConLai = 'X' THEN 1 ELSE 0 END) AS xwTrinhDoChuyenMonConLai, 		" +
                   " SUM(CASE WHEN #tmpB.THCS = 'X' THEN 1 ELSE 0 END) AS xwTHCS, 		" +
                   " SUM(CASE WHEN #tmpB.THPT = 'X' THEN 1 ELSE 0 END) AS xwTHPT, 		" +
                   " SUM(CASE WHEN #tmpB.CuNhanChinhTri = 'X' THEN 1 ELSE 0 END) AS xwCuNhanChinhTri, 		" +
                   " SUM(CASE WHEN #tmpB.CaoCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xwCaoCapChinhTri, 		" +
                   " SUM(CASE WHEN #tmpB.TrungCapChinhTri = 'X' THEN 1 ELSE 0 END) AS xwTrungCapChinhTri, 		" +
                   " SUM(CASE WHEN #tmpB.DaiHocTinHoc = 'X' THEN 1 ELSE 0 END) AS xwDaiHocTinHoc, 		" +
                   " SUM(CASE WHEN #tmpB.CaoDangTinHoc = 'X' THEN 1 ELSE 0 END) AS xwCaoDangTinHoc, 		" +
                   " SUM(CASE WHEN #tmpB.TrungCapTinHoc = 'X' THEN 1 ELSE 0 END) AS xwTrungCapTinHoc, 		" +
                   " SUM(CASE WHEN #tmpB.ChungChiTinHoc = 'X' THEN 1 ELSE 0 END) AS xwChungChiTinHoc, 		" +
                   " SUM(CASE WHEN #tmpB.DaiHocTiengAnh = 'X' THEN 1 ELSE 0 END) AS xwDaiHocTiengAnh, 		" +
                   " SUM(CASE WHEN #tmpB.ChungChiTiengAnh = 'X' THEN 1 ELSE 0 END) AS xwChungChiTiengAnh, 		" +
                   " SUM(CASE WHEN #tmpB.DaiHocNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xwDaiHocNgoaiNguKhac, 		" +
                   " SUM(CASE WHEN #tmpB.ChungChiNgoaiNguKhac = 'X' THEN 1 ELSE 0 END) AS xwChungChiNgoaiNguKhac, 		" +
                   " SUM(CASE WHEN #tmpB.ChuyenVienChinh = 'X' THEN 1 ELSE 0 END) AS  xwChuyenVienChinh, 		" +
                   " SUM(CASE WHEN #tmpB.ChuyenVien = 'X' THEN 1 ELSE 0 END) AS xwChuyenVien, 		" +
                   " SUM(CASE WHEN #tmpB.CanSu = 'X' THEN 1 ELSE 0 END) AS xwCanSu, 		" +
                   " SUM(CASE WHEN #tmpB.NgachConLai = 'X' THEN 1 ELSE 0 END) AS xwNgachConLai		" +
                   " FROM #tmpB	" +
                   " ) AS TMP ON 1=1 	" +
                   " SELECT * FROM #tmpB " +
                   " ORDER BY EmployeeTypeId";

            return sql;
        }

        /// <summary>
        /// Khai trình lao động
        /// </summary>
        /// <param name="department"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_LaborList(string department, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            sql += " SELECT rc.FullName,	 " +
                   " YEAR(rc.BirthDate) AS Year,	 " +
                   " rc.EmployeeCode,	 " +
                   " CASE WHEN rc.Sex = 1 THEN 'X' else '' END AS SexMale, " +
                   " CASE WHEN rc.Sex = 0 THEN 'X' else '' END AS SexFemale, " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT WHERE DT.Id = rc.EducationId) = 'DH' THEN 'X' ELSE '' END AS University,	 " + // filter group education
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT WHERE DT.Id = rc.EducationId) = 'CD' THEN 'X' ELSE '' END AS College,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT WHERE DT.Id = rc.EducationId) = 'TC' THEN 'X' ELSE '' END AS Intermadiate,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT WHERE DT.Id = rc.EducationId) = 'SC' THEN 'X' ELSE '' END AS PrimaryEducation,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT WHERE DT.Id = rc.EducationId) = 'CQDT' THEN 'X' ELSE '' END AS UnTraining,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT WHERE DT.Id = rc.EducationId) = 'DNTX' THEN 'X' ELSE '' END AS Vocational,	 " +
                   " CASE WHEN (SELECT TOP 1 cc.[Group] FROM cat_ContractType cc LEFT JOIN hr_Contract hc ON cc.Id = hc.ContractTypeId WHERE hc.RecordId = rc.Id) = 'KXDTH' THEN 'X' ELSE '' END AS IndefinitContract,	 " + // filter contracttype
                   " CASE WHEN (SELECT TOP 1 cc.[Group] FROM cat_ContractType cc LEFT JOIN hr_Contract hc ON cc.Id = hc.ContractTypeId WHERE hc.RecordId = rc.Id) = 'XDTH' THEN 'X' ELSE '' END AS TermContract,	 " +
                   " CASE WHEN (SELECT TOP 1 cc.[Group] FROM cat_ContractType cc LEFT JOIN hr_Contract hc ON cc.Id = hc.ContractTypeId WHERE hc.RecordId = rc.Id) = 'HDTV' THEN 'X' ELSE '' END AS SeasonContract,	 " +
                   " p.Name AS PositionName,	 " +
                   " dv.Id AS DepartmentId," +
                   " dv.Name AS DepartmentName," +
                   " rc.ParticipationDate	 " +
                   " FROM hr_Record rc	 " +
                   " LEFT JOIN cat_Position p ON rc.PositionId = p.Id	 " + // filter position
                   " LEFT JOIN cat_Department dv ON dv.Id = rc.DepartmentId"; // filter deparment
            if(!string.IsNullOrEmpty(department))
            {
                sql += " WHERE rc.DepartmentId IN ({0})".FormatWith(department);
            }
            if(!string.IsNullOrEmpty(fromDate))
            {
                sql += " AND rc.ParticipationDate >= '{0}'".FormatWith(fromDate);
            }
            if(!string.IsNullOrEmpty(toDate))
            {
                sql += " AND rc.ParticipationDate <= '{0}'".FormatWith(toDate);
            }
            if(!string.IsNullOrEmpty(condition))
            {
                sql += " AND {0}".FormatWith(condition);
            }

            return sql;
        }

        /// <summary>
        /// Khai trình tăng lao động
        /// </summary>
        /// <param name="department"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_LabourIncrease(string department, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            sql += "  IF OBJECT_ID('tempdb..#tmpA') IS NOT NULL DROP Table #tmpA" +
                   " SELECT " +
                   " CASE WHEN rc.Sex = 1 THEN '' ELSE 'X' END AS Female, " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'DH' THEN 'X' ELSE '' END AS University,	 " + // filter education
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'CD' THEN 'X' ELSE '' END AS College,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'TC' THEN 'X' ELSE '' END AS Intermediate,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'SC' THEN 'X' ELSE '' END AS PrimaryEducation,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'CQDT' THEN 'X' ELSE '' END AS UnTraining,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'DNTX' THEN 'X' ELSE '' END AS Vocational,	 " +
                   " CASE WHEN (SELECT TOP 1 cc.[Group] FROM cat_ContractType cc LEFT JOIN hr_Contract hc ON cc.Id = hc.ContractTypeId WHERE hc.RecordId = rc.Id) = 'KXDTH' THEN 'X' ELSE '' END AS IndefinitContract,	 " + // filter contract type
                   " CASE WHEN (SELECT TOP 1 cc.[Group] FROM cat_ContractType cc LEFT JOIN hr_Contract hc ON cc.Id = hc.ContractTypeId WHERE hc.RecordId = rc.Id) = 'XDTH' THEN 'X' ELSE '' END AS TermContract,	 " +
                   " CASE WHEN (SELECT TOP 1 cc.[Group] FROM cat_ContractType cc LEFT JOIN hr_Contract hc ON cc.Id = hc.ContractTypeId WHERE hc.RecordId = rc.Id) = 'HDTV' THEN 'X' ELSE '' END AS SeasonContract,	 " +
                   " dv.Id as DepartmentId," +
                   " dv.Name as DepartmentName," +
                   " 0 AS xFemale, " +
                   " 0 AS xUniversity," +
                   " 0 AS xColleage," +
                   " 0 AS xIntermediate," +
                   " 0 AS xPrimaryEducation," +
                   " 0 AS xUnTraining," +
                   " 0 AS xVocational," +
                   " 0 AS xIndefinitContract," +
                   " 0 AS xTermContract," +
                   " 0 AS xSeasonContract" +
                   " INTO #tmpA" +
                   " FROM hr_FluctuationEmployee fe 	 " +
                   " left join hr_Record rc on rc.Id = fe.RecordId	 " +
                   " LEFT JOIN cat_Department dv ON dv.Id = rc.DepartmentId" + // filter department
                   " WHERE fe.Type = 0 ";
            if(!string.IsNullOrEmpty(department))
            {
                sql += " AND rc.DepartmentId IN ({0})".FormatWith(department);
            }
            if(!string.IsNullOrEmpty(fromDate))
            {
                sql += " AND fe.Date >= '{0}'".FormatWith(fromDate);
            }
            if(!string.IsNullOrEmpty(toDate))
            {
                sql += " AND fe.Date <= '{0}'".FormatWith(toDate);
            }
            if(!string.IsNullOrEmpty(condition))
            {
                sql += " AND {0}".FormatWith(condition);
            }
            sql += "UPDATE #tmpA" +
                   " SET xFemale = totalFemale," +
                   " xUniversity = totalUniversity," +
                   " xColleage = totalColleage," +
                   " xIntermediate = totalIntermediate," +
                   " xPrimaryEducation = totalPrimaryEducation," +
                   " xUnTraining = totalUnTraining," +
                   " xVocational = totalVocational," +
                   " xIndefinitContract = totalIndefinitContract," +
                   " xTermContract = totalTermContract," +
                   " xSeasonContract = totalSeasonContract" +
                   " FROM #tmpA" +
                   " INNER JOIN(" +
                   " SELECT SUM(CASE WHEN #tmpA.Female = 'X' THEN 1 ELSE 0 END) AS totalFemale," +
                   " SUM(CASE WHEN #tmpA.University = 'X' THEN 1 ELSE 0 END) AS totalUniversity," +
                   " SUM(CASE WHEN #tmpA.College = 'X' THEN 1 ELSE 0 END) AS totalColleage," +
                   " SUM(CASE WHEN #tmpA.Intermediate = 'X' THEN 1 ELSE 0 END) AS totalIntermediate," +
                   " SUM(CASE WHEN #tmpA.PrimaryEducation = 'X' THEN 1 ELSE 0 END) AS totalPrimaryEducation," +
                   " SUM(CASE WHEN #tmpA.UnTraining = 'X' THEN 1 ELSE 0 END) AS totalUnTraining," +
                   " SUM(CASE WHEN #tmpA.Vocational = 'X' THEN 1 ELSE 0 END) AS totalVocational," +
                   " SUM(CASE WHEN #tmpA.IndefinitContract = 'X' THEN 1 ELSE 0 END) AS totalIndefinitContract," +
                   " SUM(CASE WHEN #tmpA.TermContract = 'X' THEN 1 ELSE 0 END) AS totalTermContract," +
                   " SUM(CASE WHEN #tmpA.SeasonContract = 'X' THEN 1 ELSE 0 END) AS totalSeasonContract" +
                   " FROM #tmpA" +
                   " ) AS #tmp ON 1=1" +

                   " SELECT * FROM #tmpA";

            return sql;
        }

        /// <summary>
        /// Khai trình giảm lao động
        /// </summary>
        /// <param name="department"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_LabourDecrease(string department, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            sql += " IF OBJECT_ID('tempdb..#tmpA') IS NOT NULL DROP Table #tmpA " +
                   " SELECT " +
                   " CASE WHEN rc.Sex = 1 THEN '' ELSE 'X' END AS Female, " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'DH' THEN 'X' ELSE '' END AS University,	 " + //filter education
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'CD' THEN 'X' ELSE '' END AS College,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'TC' THEN 'X' ELSE '' END AS Intermediate,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'SC' THEN 'X' ELSE '' END AS PrimaryEducation,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'CQDT' THEN 'X' ELSE '' END AS UnTraining,	 " +
                   " CASE WHEN (SELECT TOP 1 [Group] FROM cat_Education DT  WHERE DT.Id = rc.EducationId) = 'DNTX' THEN 'X' ELSE '' END AS Vocational,	 " +
                   " CASE WHEN (SELECT TOP 1 cc.[Group] FROM cat_ContractType cc LEFT JOIN hr_Contract hc ON cc.Id = hc.ContractTypeId WHERE hc.RecordId = rc.Id) = 'KXDTH' THEN 'X' ELSE '' END AS IndefinitContract,	 " + //filter contracttype
                   " CASE WHEN (SELECT TOP 1 cc.[Group] FROM cat_ContractType cc LEFT JOIN hr_Contract hc ON cc.Id = hc.ContractTypeId WHERE hc.RecordId = rc.Id) = 'XDTH' THEN 'X' ELSE '' END AS TermContract,	 " +
                   " CASE WHEN (SELECT TOP 1 cc.[Group] FROM cat_ContractType cc LEFT JOIN hr_Contract hc ON cc.Id = hc.ContractTypeId WHERE hc.RecordId = rc.Id) = 'HDTV' THEN 'X' ELSE '' END AS SeasonContract,	 " +
                   " dv.Id AS DepartmentId," +
                   " dv.Name AS DepartmentName," +
                   " CASE WHEN (Select Count(*) FROM hr_FluctuationEmployee hf WHERE hf.RecordId = rc.Id AND hf.Reason LIKE N'{0}') > 0 THEN 'X' ELSE '' END AS ReasonRetirement, "
                       .FormatWith(ReasonRetirement) +
                   " CASE WHEN (Select Count(*) FROM hr_FluctuationEmployee hf WHERE hf.RecordId = rc.Id AND hf.Reason LIKE N'{0}') > 0 THEN 'X' ELSE '' END AS ReasonExpiredContract, "
                       .FormatWith(ReasonExpiredContract) +
                   " CASE WHEN (Select Count(*) FROM hr_FluctuationEmployee hf WHERE hf.RecordId = rc.Id AND hf.Reason LIKE N'{0}') > 0 THEN 'X' ELSE '' END AS ReasonFired, "
                       .FormatWith(ReasonFired) +
                   " CASE WHEN (Select Count(*) FROM hr_FluctuationEmployee hf WHERE hf.RecordId = rc.Id AND hf.Reason LIKE N'{0}') > 0 THEN 'X' ELSE '' END AS ReasonOther, "
                       .FormatWith(ReasonOther) +
                   " CASE WHEN (Select Count(*) FROM hr_FluctuationEmployee hf WHERE hf.RecordId = rc.Id AND hf.Reason LIKE N'{0}')  > 0 THEN 'X' ELSE '' END AS ReasonTerminate, "
                       .FormatWith(ReasonTerminate) +
                   " 0 AS xFemale," +
                   " 0 AS xUniversity," +
                   " 0 AS xColleage," +
                   " 0 AS xIntermediate," +
                   " 0 AS xPrimaryEducation," +
                   " 0 AS xVocational," +
                   " 0 AS xUnTraining," +
                   " 0 AS xIndefinitContract," +
                   " 0 AS xTermContract," +
                   " 0 AS xSeasonContract," +
                   " 0 AS xReasonRetirement," +
                   " 0 AS xReasonExpiredContract," +
                   " 0 AS xReasonFired," +
                   " 0 As xReasonOther," +
                   " 0 As xReasonTerminate" +
                   " into #tmpA " +
                   " FROM hr_FluctuationEmployee fe 	 " +
                   " LEFT JOIN hr_Record rc on rc.Id = fe.RecordId	 " +
                   " LEFT JOIN cat_Department dv ON dv.Id = rc.DepartmentId" + // filter deparment
                   " WHERE fe.Type = 1 ";
            if(!string.IsNullOrEmpty(department))
            {
                sql += " AND rc.DepartmentId IN ({0})".FormatWith(department);
            }
            if(!string.IsNullOrEmpty(fromDate))
            {
                sql += " AND fe.Date >= '{0}'".FormatWith(fromDate);
            }
            if(!string.IsNullOrEmpty(toDate))
            {
                sql += " AND fe.Date <= '{0}'".FormatWith(toDate);
            }
            if(!string.IsNullOrEmpty(condition))
            {
                sql += " AND {0}".FormatWith(condition);
            }
            sql += "UPDATE #tmpA" +
                   " SET xFemale = totalFemale," +
                   " xUniversity = totalUniversity," +
                   " xColleage = totalColleage," +
                   " xIntermediate = totalIntermediate," +
                   " xPrimaryEducation = totalPrimaryEducation," +
                   " xUnTraining = totalUnTraining," +
                   " xVocational = totalVocational," +
                   " xIndefinitContract = totalIndefinitContract," +
                   " xTermContract = totalTermContract," +
                   " xSeasonContract = totalSeasonContract, " +
                   "  xReasonRetirement = totalReasonRetirement, " +
                   " xReasonExpiredContract = totalReasonExpiredContract," +
                   " xReasonFired = totalReasonFired," +
                   " xReasonOther = totalReasonOther," +
                   " xReasonTerminate = totalReasonTerminate" +
                   " FROM #tmpA" +
                   " INNER JOIN(" +
                   " SELECT SUM(CASE WHEN #tmpA.Female = 'X' THEN 1 ELSE 0 END) AS totalFemale," +
                   " SUM(CASE WHEN #tmpA.University = 'X' THEN 1 ELSE 0 END) AS totalUniversity," +
                   " SUM(CASE WHEN #tmpA.College = 'X' THEN 1 ELSE 0 END) AS totalColleage," +
                   " SUM(CASE WHEN #tmpA.Intermediate = 'X' THEN 1 ELSE 0 END) AS totalIntermediate," +
                   " SUM(CASE WHEN #tmpA.PrimaryEducation = 'X' THEN 1 ELSE 0 END) AS totalPrimaryEducation," +
                   " SUM(CASE WHEN #tmpA.UnTraining = 'X' THEN 1 ELSE 0 END) AS totalUnTraining," +
                   " SUM(CASE WHEN #tmpA.Vocational = 'X' THEN 1 ELSE 0 END) AS totalVocational," +
                   " SUM(CASE WHEN #tmpA.IndefinitContract = 'X' THEN 1 ELSE 0 END) AS totalIndefinitContract," +
                   " SUM(CASE WHEN #tmpA.TermContract = 'X' THEN 1 ELSE 0 END) AS totalTermContract," +
                   " SUM(CASE WHEN #tmpA.SeasonContract = 'X' THEN 1 ELSE 0 END) AS totalSeasonContract," +

                   " SUM(CASE WHEN #tmpA.ReasonRetirement = 'X' THEN 1 ELSE 0 END) AS totalReasonRetirement," +
                   " SUM(CASE WHEN #tmpA.ReasonExpiredContract = 'X' THEN 1 ELSE 0 END) AS totalReasonExpiredContract," +
                   " SUM(CASE WHEN #tmpA.ReasonFired = 'X' THEN 1 ELSE 0 END) AS totalReasonFired," +
                   " SUM(CASE WHEN #tmpA.ReasonOther = 'X' THEN 1 ELSE 0 END) AS totalReasonOther," +
                   " SUM(CASE WHEN #tmpA.ReasonTerminate = 'X' THEN 1 ELSE 0 END) AS totalReasonTerminate" +
                   " FROM #tmpA" +
                   ") AS #tmp ON 1=1" +

                   " SELECT * FROM #tmpA";

            return sql;
        }

        /// <summary>
        /// Danh sách nhân sự
        /// </summary>
        /// <param name="department"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_EmployeeList(string department, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            sql += "IF OBJECT_ID('tempdb..#tmpA') IS NOT NULL DROP Table #tmpA" +
                   " SELECT rc.FullName, " +
                   " rc.BirthDate, " +
                   " rc.EmployeeCode, " +
                   " p.Name AS PositionName, " +
                   " dv.Id AS DepartmentId, " +
                   " dv.Name AS DepartmentName, " +
                   " (SELECT TOP 1 dt.Name FROM cat_Education dt WHERE dt.Id = rc.EducationId) AS Certify, " + // filter education
                   " rc.CellPhoneNumber, " +
                   " CASE WHEN (SELECT top 1 cw.Id FROM cat_WorkStatus cw WHERE cw.Id = rc.WorkStatusId) = (SELECT  TOP 1 Id FROM cat_WorkStatus WHERE [Name] LIKE N'{0}') then 1 ".FormatWith(WorkStatusWorking) +
                   "  ELSE 0 END AS Working, " +
                   " CASE WHEN (SELECT top 1 cw.Id FROM cat_WorkStatus cw WHERE cw.Id = rc.WorkStatusId) = (SELECT  TOP 1 Id FROM cat_WorkStatus WHERE [Name] LIKE N'{0}') then 1 ".FormatWith(WorkStatusLeave) +
                   "  ELSE 0 END AS Leave, " +
                   " CASE WHEN (SELECT TOP 1 cw.Id FROM cat_WorkStatus cw WHERE cw.Id = rc.WorkStatusId) = (SELECT  TOP 1 Id FROM cat_WorkStatus WHERE [Name] LIKE N'{0}') then 1 ".FormatWith(WorkStatusWorking) +
                   " WHEN (SELECT TOP 1 cw.Id FROM cat_WorkStatus cw WHERE cw.Id = rc.WorkStatusId) = (SELECT TOP 1 Id FROM cat_WorkStatus WHERE [Name] LIKE N'{0}') then 1 ".FormatWith(WorkStatusLeave) +
                   " ELSE 0 END AS Total, " +
                   " '' AS Note," +
                   " 0 AS xEmployee " +
                   " INTO #tmpA" +
                   " FROM hr_Record rc " +
                   " LEFT JOIN cat_Position p ON rc.PositionId = p.Id " + // filter position
                   " LEFT JOIN cat_Department dv ON rc.DepartmentId  = dv.Id " + // filter department
                   " WHERE 1 = 1 ";
            if(!string.IsNullOrEmpty(department))
            {
                sql += " AND rc.DepartmentId IN ({0})".FormatWith(department);
            }
            if(!string.IsNullOrEmpty(fromDate))
            {
                sql += " AND rc.ParticipationDate >= '{0}'".FormatWith(fromDate);
            }
            if(!string.IsNullOrEmpty(toDate))
            {
                sql += " AND rc.ParticipationDate <= '{0}'".FormatWith(toDate);
            }
            if(!string.IsNullOrEmpty(condition))
            {
                sql += " AND {0}".FormatWith(condition);
            }
            sql +=
                " UPDATE #tmpA " +
                " SET xEmployee = totalEmployee" +
                " FROM #tmpA" +
                " INNER JOIN (SELECT COUNT(EmployeeCode) AS totalEmployee FROM #tmpA) AS #tmp ON 1=1" +
                " SELECT * FROM #tmpA";
            return sql;
        }

        /// <summary>
        /// Báo cáo tăng/giảm lao động
        /// </summary>
        /// <param name="department"></param>
        /// <param name="direction"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_EmployeeAdjust(string department, bool direction, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            sql += " IF OBJECT_ID('tempdb..#tmpA') IS NOT NULL DROP Table #tmpA " +
                   " SELECT rc.FullName,	" +
                   " rc.BirthDate,	" +
                   " rc.EmployeeCode,	" +
                   " p.Name AS PositionName,	" +
                   " dv.Id AS DepartmentId,	" +
                   " dv.Name AS DepartmentName,	" +
                   " (SELECT TOP 1 dt.Name FROM cat_Education dt  WHERE rc.EducationId = dt.Id) AS Certify, " + // filter education
                   " hf.Date,	" +
                   " 0 AS xEmployee" +
                   " INTO #tmpA " +
                   " FROM hr_FluctuationEmployee hf	" +
                   " LEFT JOIN hr_Record rc ON hf.RecordId = rc.Id	" + 
                   " LEFT JOIN cat_Position p on rc.PositionId = p.Id	" + // filter postion
                   " LEFT JOIN cat_Department dv ON dv.Id = rc.DepartmentId	" + // filter department
                   " WHERE 1 = 1 ";
            if(!string.IsNullOrEmpty(department))
            {
                sql += "AND rc.DepartmentId IN ({0})".FormatWith(department);
            }
            if(direction == false)
            {
                sql += " AND hf.Type = 0 "; //Tang
            }
            else
            {
                sql += " AND hf.Type = 1 "; //Giam
            }
            if(!string.IsNullOrEmpty(fromDate))
            {
                sql += " AND hf.Date>='{0}'".FormatWith(fromDate);
            }
            if(!string.IsNullOrEmpty(toDate))
            {
                sql += " AND hf.Date<='{0}'".FormatWith(toDate);
            }
            if(!string.IsNullOrEmpty(condition))
            {
                sql += " AND {0}".FormatWith(condition);
            }
            sql += "UPDATE #tmpA  " +
                   " SET xEmployee = totalEmployee" +
                   " FROM #tmpA " +
                   " INNER JOIN(SELECT case WHEN COUNT(EmployeeCode) > 0 THEN COUNT(EmployeeCode) ELSE 0 END AS totalEmployee FROM #tmpA) AS #tmp " +
                   " ON 1 = 1 " +
                   "SELECT * FROM #tmpA";

            return sql;
        }

        /// <summary>
        /// Báo cáo điều động nhân sự
        /// </summary>
        /// <param name="department"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_EmployeeTransferred(string department, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            sql += " SELECT " +
                   " rc.FullName, " +
                   " rc.EmployeeCode, " +
                   " hb.CurrentPosition, " +
                   " cc.Name AS CurrentProjectName, " +
                   " '' AS NewProjectName, " +
                   " hb.NewPosition, " +
                   " dv.Id AS DepartmentId, " +
                   " dv.Name AS DepartmentName, " +
                   " hb.DecisionDate " +
                   " FROM hr_BusinessHistory hb " +
                   " LEFT JOIN hr_Record rc ON hb.RecordId = rc.Id " +
                   " LEFT JOIN cat_Department dv ON dv.Id = rc.DepartmentId	" + // filter department
                   " LEFT JOIN hr_Team ht on ht.RecordId = rc.Id " + // TODO : filter ?
                   " LEFT JOIN cat_Construction cc on cc.Id = ht.ConstructionId " + // TODO : filter ?
                   " WHERE 1=1  ";
            if(!string.IsNullOrEmpty(BusinessPersonelRotation))
                sql += " AND hb.BusinessType = '{0}' ".FormatWith(BusinessPersonelRotation);
            if(!string.IsNullOrEmpty(department))
                sql += " AND rc.DepartmentId IN ({0}) ".FormatWith(department);
            if(!string.IsNullOrEmpty(fromDate))
            {
                sql += " AND hb.DecisionDate >= '{0}'".FormatWith(fromDate);
            }
            if(!string.IsNullOrEmpty(toDate))
            {
                sql += " AND hb.DecisionDate <= '{0}'".FormatWith(toDate);
            }

            if(!string.IsNullOrEmpty(condition))
            {
                sql += " AND {0}".FormatWith(condition);
            }
            return sql;
        }

        /// <summary>
        /// Báo cáo biệt phái nhân sự
        /// </summary>
        /// <param name="department"></param>
        /// <param name="businessType"></param>
        /// <param name="condition"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static string GetStore_EmployeeSent(string department, string businessType, string condition, string fromDate, string toDate)
        {
            var sql = string.Empty;
            sql += " SELECT " +
                   " rc.FullName, " +
                   " rc.EmployeeCode, " +
                   " hb.DecisionDate AS FromDate, " +
                   " hb.ExpireDate AS ToDate, " +
                   " hb.CurrentPosition, " +
                   " dv.Name AS DepartmentName, " +
                   " dv.Id AS DepartmentId, " +
                   " hb.CurrentDepartment " +
                   " FROM hr_BusinessHistory hb " +
                   " LEFT JOIN hr_Record rc ON hb.RecordId = rc.Id " +
                   " LEFT JOIN cat_Department dv ON rc.DepartmentId = dv.Id " + // filter department
                   " WHERE 1 = 1 ";
            if(!string.IsNullOrEmpty(department))
                sql += " AND rc.DepartmentId IN ({0})".FormatWith(department);
            if(!string.IsNullOrEmpty(businessType))
                sql += " AND hb.BusinessType = '{0}' ".FormatWith(businessType);
            if(!string.IsNullOrEmpty(condition))
                sql += " AND {0}".FormatWith(condition);
            if(!string.IsNullOrEmpty(fromDate))
                sql += " AND hb.DecisionDate >= '{0}'".FormatWith(fromDate);
            if(!string.IsNullOrEmpty(toDate))
                sql += " AND hb.DecisionDate <= '{0}'".FormatWith(toDate);
            return sql;
        }

        /// <summary>
        /// Báo cáo bổ nhiệm nhân sự
        /// </summary>
        /// <param name="department"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_EmployeeAssigned(string department, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            sql += " SELECT " +
                   " rc.FullName, " +
                   " rc.EmployeeCode, " +
                   " dv.Id AS DepartmentId, " +
                   " dv.Name AS DepartmentName, " +
                   " (SELECT TOP 1 name FROM cat_Position WHERE Id = hw.NewPositionId) AS NewPositionName, " + // new position
                   " cp.Name AS CurrentPositionName, " +
                   " hw.DecisionDate " +
                   " FROM hr_Record rc " +
                   " LEFT JOIN cat_Position cp ON rc.PositionId = cp.Id  " + // filter postion
                   " LEFT JOIN cat_Department dv ON rc.DepartmentId = dv.Id " + // filter department
                   " RIGHT JOIN hr_WorkProcess hw ON rc.Id = hw.RecordId " +
                   " WHERE 1=1 ";
            if(!string.IsNullOrEmpty(department))
                sql += " AND rc.DepartmentId IN ({0})".FormatWith(department);
            if(!string.IsNullOrEmpty(fromDate))
                sql += " AND hw.DecisionDate >= '{0}'".FormatWith(fromDate);
            if(!string.IsNullOrEmpty(toDate))
                sql += " AND hw.DecisionDate <= '{0}'".FormatWith(toDate);
            if(!string.IsNullOrEmpty(condition))
                sql += " AND {0}".FormatWith(condition);
            return sql;
        }

        /// <summary>
        /// Báo cáo thâm niên công tác
        /// </summary>
        /// <param name="department"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_EmployeeSeniority(string department, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;

            sql += " SELECT  " +
                   " rc.FullName, " +
                   " rc.EmployeeCode, " +
                   " YEAR(rc.BirthDate) AS Year, " +
                   " rc.Address, " +
                   " cp.Name AS PositionName, " +
                   " ce.Name AS EducationName, " +
                   " dv.Id AS DepartmentId, " +
                   " dv.Name AS DepartmentName, " +
                   " rc.CellPhoneNumber, " +
                   " rc.ParticipationDate, " +
                   "   CASE WHEN (DATEDIFF(DAY, rc.ParticipationDate, getDate())/365) = 0 THEN '' ELSE " +
                   "   CAST(DATEDIFF(DAY, rc.ParticipationDate, getDate())/365 AS NVARCHAR(10)) + N' Năm ' END + " +
                   "   CASE WHEN ((DATEDIFF(DAY, rc.ParticipationDate, getDate())%365) /30) = 0 THEN '' ELSE " +
                   "   CAST((DATEDIFF(DAY, rc.ParticipationDate, getDate())%365) /30 AS NVARCHAR(10)) + N' Tháng ' END + " +
                   "   CASE WHEN ((DATEDIFF(DAY, rc.ParticipationDate, getDate())%365) %30) = 0 THEN '' ELSE " +
                   "   CAST((DATEDIFF(DAY, rc.ParticipationDate, getDate())%365) %30 AS NVARCHAR(10)) + N' Ngày' END AS 'Seniority' " +
                   " FROM hr_Record rc " +
                   " LEFT JOIN cat_Position cp ON rc.PositionId = cp.Id " + // filter position
                   " LEFT JOIN cat_Education ce ON rc.EducationId = ce.Id " + // filter education
                   " LEFT JOIN cat_Department dv ON rc.EducationId = dv.Id " + // filter department
                   " WHERE 1=1 ";
            if(!string.IsNullOrEmpty(department))
                sql += " AND rc.DepartmentId IN ({0})".FormatWith(department);
            if(!string.IsNullOrEmpty(fromDate))
                sql += " AND rc.ParticipationDate >= '{0}'".FormatWith(fromDate);
            if(!string.IsNullOrEmpty(toDate))
                sql += " AND rc.ParticipationDate <= '{0}'".FormatWith(toDate);
            if(!string.IsNullOrEmpty(condition))
            {
                sql += " AND {0}".FormatWith(condition);
            }
            return sql;
        }

        /// <summary>
        /// Báo cáo nhân sự nghỉ hưu
        /// </summary>
        /// <param name="department"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetStore_EmployeeRetired(string department, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;
            sql += " SELECT " +
                   " rc.EmployeeCode, " +
                   " rc.FullName, " +
                   " cp.Name AS PositionName, " +
                   " dv.Id AS DepartmentId, " +
                   " dv.Name AS DepartmentName, " +
                   " bh.DecisionNumber, " +
                   " bh.DecisionDate, " +
                   "   CASE WHEN (DATEDIFF(DAY, rc.ParticipationDate, getDate())/365) = 0 THEN '' ELSE " +
                   "   CAST(DATEDIFF(DAY, rc.ParticipationDate, getDate())/365 AS NVARCHAR(10)) + N' Năm ' END + " +
                   "   CASE WHEN ((DATEDIFF(DAY, rc.ParticipationDate, getDate())%365) /30) = 0 THEN '' ELSE " +
                   "   CAST((DATEDIFF(DAY, rc.ParticipationDate, getDate())%365) /30 AS NVARCHAR(10)) + N' Tháng ' END + " +
                   "   CASE WHEN ((DATEDIFF(DAY, rc.ParticipationDate, getDate())%365) %30) = 0 THEN '' ELSE " +
                   "   CAST((DATEDIFF(DAY, rc.ParticipationDate, getDate())%365) %30 AS NVARCHAR(10)) + N' Ngày' END AS 'Seniority' " +
                   " FROM hr_BusinessHistory bh " +
                   " LEFT JOIN hr_Record rc ON bh.RecordId = rc.Id " +
                   " LEFT JOIN cat_Position cp ON rc.PositionId = cp.Id " + // filter position
                   " LEFT JOIN cat_Department dv ON rc.DepartmentId = dv.Id " + // filter department
                   " WHERE bh.BusinessType = '{0}' ".FormatWith(BusinessTypeRetirement) +
                   " AND bh.DecisionDate IS NOT NULL ";

            if(!string.IsNullOrEmpty(department))
                sql += " AND rc.DepartmentId IN ({0})".FormatWith(department);
            if(!string.IsNullOrEmpty(condition))
                sql += " AND {0}".FormatWith(condition);
            if(!string.IsNullOrEmpty(fromDate))
                sql += " AND bh.DecisionDate >= '{0}'".FormatWith(fromDate);
            if(!string.IsNullOrEmpty(toDate))
                sql += " AND bh.DecisionDate <= '{0}'".FormatWith(toDate);
            return sql;
        }


        /// <summary>
        /// Báo cáo danh sách cán bộ quản lý, giáo viên và nhân viên trong các đơn vị trường học
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static string GetStore_ListEmployeeByPosition(string departments, string fromDate, string toDate, string condition)
        {
            var sql = string.Empty;

            sql += "	SELECT DISTINCT th.TrainingPlace,	" +
                   " cml.Name AS 'ManagerLevel'," +
                   " ISNULL(cl1.Name, '-') + ' - ' + ISNULL(cl2.Name, '-') + ' - ' + ISNULL(cl3.Name, '-') AS Hometown," +
                   " rc.FunctionaryDate," +
                   " rc.RecruimentDate," +
                   " rc.CPVJoinedDate AS 'CPVJoinedDate'," +
                   " cp.Id AS 'Code'," +
                   " cll.Name AS 'Language'," +
                   " cil.Name AS 'It'," +
                   " cpl.Name AS 'Political'," +
                   " cf.Name AS 'Nation'," +
                   " rc.DepartmentId AS 'UnitCode'," +
                   " dp.Name AS 'WorkUnit'," +
                   " '' AS 'STT'," +
                   " rc.FullName AS 'FullName'," +
                   " rc.BirthDate AS 'Birthday'," +
                   " CASE" +
                   " 	WHEN rc.Sex = '1' THEN ''								" +
                   " 	ELSE N'x'								" +
                   " END AS 'Sex'," +
                   " rc.Address AS 'Address'," +
                   " cp.Name AS 'Position'," +
                   "	  (SELECT TOP 1 cq.Code	" +
                   "	   FROM cat_Quantum cq,	" +
                   "	        sal_SalaryDecision hr	" +
                   "	   WHERE hr.QuantumId = cq.Id	" +
                   "	     AND hr.RecordId = rc.Id) AS 'CodeQuantum',	" +
                   "	  (SELECT TOP 1 ce.Name	" +
                   "	   FROM cat_Education ce, hr_EducationHistory eh	" +
                   "	   WHERE eh.EducationId = ce.Id	" +
                   "	    AND eh.RecordId = rc.Id	" +
                   "	   ORDER BY eh.FromDate DESC) AS 'CurrentDegree',	" +
                   "	  (SELECT TOP 1 ce.Name	" +
                   "	   FROM cat_Education ce, hr_EducationHistory eh	" +
                   "	   WHERE eh.EducationId = ce.Id	" +
                   "	    AND eh.RecordId = rc.Id	" +
                   "	   ORDER BY eh.FromDate) AS 'OldDegree',	" +
                   "	  (SELECT TOP 1 cts.Name	" +
                   "	   FROM cat_TrainingSystem cts	" +
                   "	   WHERE th.TrainingSystemId = cts.Id	" +
                   "	   ORDER BY th.StartDate) AS 'TrainingSystemName',	" +
                   "	  (SELECT TOP 1 cu.Name	" +
                   "	   FROM cat_University cu, hr_EducationHistory eh	" +
                   "	   WHERE eh.UniversityId = cu.Id	" +
                   "	   AND eh.RecordId = rc.Id	" +
                   "	   ORDER BY eh.FromDate) AS 'UniversityName',	" +
                   "	  (SELECT TOP 1 ci.Name	" +
                   "	   FROM cat_Industry ci, hr_EducationHistory eh	" +
                   "	   WHERE eh.IndustryId = ci.Id	" +
                   "	   AND eh.RecordId = rc.Id	" +
                   "	   ORDER BY eh.FromDate) AS 'IndustryName',	" +
                   "	  (SELECT TOP 1 cu.Name	" +
                   "	   FROM cat_University cu, hr_EducationHistory eh	" +
                   "	   WHERE cu.Id = eh.UniversityId	" +
                   "	    AND eh.RecordId = rc.Id	" +
                   "	   ORDER BY eh.FromDate DESC) AS 'CurrentPlaceOfTraining',	" +
                   "	  (SELECT TOP 1 ci.Name	" +
                   "	   FROM cat_Industry ci, hr_EducationHistory eh	" +
                   "	   WHERE eh.IndustryId = ci.Id	" +
                   "	    AND eh.RecordId = rc.Id	" +
                   "	   ORDER BY eh.FromDate DESC) AS 'CurrentSpecialized'	" +
                   "	FROM hr_Record rc	" +
                   "	LEFT JOIN hr_EducationHistory eh ON eh.RecordId = rc.Id	" +
                   "	LEFT JOIN hr_TrainingHistory th ON th.RecordId = rc.Id	" +
                   "	LEFT JOIN hr_Contract hc ON hc.RecordId = rc.Id	" +
                   "	LEFT JOIN cat_ManagementLevel cml ON rc.ManagementLevelId = cml.Id	" +
                   "	LEFT JOIN cat_Location cl1 ON rc.HometownWardId = cl1.Id	" +
                   "	LEFT JOIN cat_Location cl2 ON rc.HometownDistrictId = cl2.Id	" +
                   "	LEFT JOIN cat_Location cl3 ON rc.HometownProvinceId = cl3.Id	" +
                   "	LEFT JOIN cat_LanguageLevel cll ON rc.LanguageLevelId = cll.Id	" + // filter language level
                   "	LEFT JOIN cat_PoliticLevel cpl ON rc.PoliticLevelId = cpl.Id	" + // filter politic level
                   "	LEFT JOIN cat_ITLevel cil ON rc.ITLevelId = cil.Id	" + // filter it level
                   "	LEFT JOIN cat_Folk cf ON rc.FolkId = cf.Id	" + // fillter fold
                   "	LEFT JOIN cat_JobTitle cjt ON rc.JobTitleId = cjt.Id	" + // filter jobtitle
                   "	LEFT JOIN cat_Position cp ON rc.PositionId = cp.Id	" + // filter position
                   "	LEFT JOIN cat_ContractType ct ON hc.ContractTypeId = ct.Id	" + // filter contract type
                   "	LEFT JOIN cat_Department dp ON rc.DepartmentId = dp.Id	" + //  filer department
                   "   WHERE rc.DepartmentId IN ({0})".FormatWith(departments);

                    if(!string.IsNullOrEmpty(fromDate))
                    {
                        sql += " AND rc.RecruimentDate >= '{0}'".FormatWith(fromDate);
                    }
                    if(!string.IsNullOrEmpty(toDate))
                    {
                        sql += " AND rc.RecruimentDate <= '{0}'".FormatWith(toDate);
                    }
                    if(!string.IsNullOrEmpty(condition))
                    {
                        sql += " AND {0}".FormatWith(condition);
                    }
                    return sql;

        }

    }
}
