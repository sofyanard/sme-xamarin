using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMEXamarin.Models
{
    public class RefPropinsi
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class RefKotaKab
    {
        [Key]
        public string Id { get; set; }

        public string PropId { get; set; }

        [ForeignKey("PropId")]
        public virtual RefPropinsi RefPropinsi { get; set; }

        public string Name { get; set; }
    }

    public class RefKecamatan
    {
        [Key]
        public string Id { get; set; }

        public string KotaId { get; set; }

        [ForeignKey("KotaId")]
        public virtual RefKotaKab RefKotaKab { get; set; }

        public string Name { get; set; }
    }

    public class RefKelurahan
    {
        [Key]
        public string Id { get; set; }

        public string KecId { get; set; }

        [ForeignKey("KecId")]
        public virtual RefKecamatan RefKecamatan { get; set; }

        public string Name { get; set; }
    }

    public class RfSex
    {
        [Key]
        [Column("SEXID")]
        public string SexId { get; set; }

        [Column("SEXDESC")]
        public string SexDesc { get; set; }
    }

    public class RfEducation
    {
        [Key]
        [Column("EDUCATIONID")]
        public string EducationId { get; set; }

        [Column("EDUCATIONDESC")]
        public string EducationDesc { get; set; }
    }

    public class RfMarital
    {
        [Key]
        [Column("MARITALID")]
        public string MaritalId { get; set; }

        [Column("MARITALDESC")]
        public string MaritalDesc { get; set; }
    }

    public class RfCitizenship
    {
        [Key]
        [Column("CITIZENID")]
        public string CitizenId { get; set; }

        [Column("CITIZENDESC")]
        public string CitizenDesc { get; set; }
    }

    public class RfHomeStatus
    {
        [Key]
        [Column("HM_CODE")]
        public string HomeStatusId { get; set; }

        [Column("HM_DESC")]
        public string HomeStatusDesc { get; set; }
    }

    public class RfJobTitle
    {
        [Key]
        [Column("JOBTITLEID")]
        public string JobTitleId { get; set; }

        [Column("JOBTITLEDESC")]
        public string JobTitleDesc { get; set; }
    }

    public class RfRelationship
    {
        [Key]
        [Column("RELTYPEID")]
        public string RelationshipId { get; set; }

        [Column("RELTYPEDESC")]
        public string RelationshipDesc { get; set; }
    }

    public class RfProduct
    {
        [Key]
        [Column("PRODUCTID")]
        public string ProductId { get; set; }

        [Column("PRODUCTDESC")]
        public string ProductDesc { get; set; }

        [Column("ISSUBAPPPROD")]
        public string IsSubAppProd { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    public class RfLoanPurpose
    {
        [Key]
        [Column("LOANPURPID")]
        public string LoanPurpId { get; set; }

        [Column("LOANPURPDESC")]
        public string LoanPurpDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    public class RfCollateralType
    {
        [Key]
        [Column("COLTYPESEQ")]
        public int ColTypeSeq { get; set; }

        [Column("COLTYPEID")]
        public string ColTypeId { get; set; }

        [Column("COLTYPEDESC")]
        public string ColTypeDesc { get; set; }

        [Column("COLLINKTABLE")]
        public string ColLinkTable { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    public class RfCertType
    {
        [Key]
        [Column("CERTTYPEID")]
        public string CertTypeId { get; set; }

        [Column("CERTTYPEDESC")]
        public string CertTypeDesc { get; set; }

        [Column("COLFLAG")]
        public string ColFlag { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    public class RfArea
    {
        [Key]
        [Column("AREAID")]
        public string AreaId { get; set; }

        [Column("AREANAME")]
        public string AreaName { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    public class RfCity
    {
        [Key]
        [Column("CITYID")]
        public string CityId { get; set; }

        [Column("AREAID")]
        public string AreaId { get; set; }

        [ForeignKey("AreaId")]
        public virtual RfArea RfArea { get; set; }

        [Column("CITYNAME")]
        public string CityName { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    public class RfBranch
    {
        [Key]
        [Column("BRANCH_CODE")]
        public string BranchCode { get; set; }

        [Column("CITYID")]
        public string CityId { get; set; }

        [ForeignKey("CityId")]
        public virtual RfCity RfCity { get; set; }

        [Column("BRANCH_NAME")]
        public string BranchName { get; set; }

        [Column("BR_ISBRANCH")]
        public string IsBranch { get; set; }

        [Column("BRANCH_TYPE")]
        public string BranchType { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    public class RfTrack
    {
        [Key]
        [Column("TRACKCODE")]
        public string TrackCode { get; set; }

        [Column("TRACKNAME")]
        public string TrackName { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    public class RfApplicationType
    {
        [Key]
        [Column("APPTYPEID")]
        public string AppTypeId { get; set; }

        [Column("APPTYPEDESC")]
        public string AppTypeDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }
}
