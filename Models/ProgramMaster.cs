namespace ActivitiesManagement.Models
{
    public class ProgramMaster
    {
        public long Id { get; set; }
        public string? ProgramCode { get; set; }
        public string Title { get; set; } = string.Empty;
        public long? ProgramTypeId { get; set; }
        public string? ProgramTypeTitle { get; set; }
        public long? CountryId { get; set; }
        public long? InstituteTypeId { get; set; }
        public string? InstituteTypeTitle { get; set; }
        public long? InstituteId { get; set; }
        public string? InstituteTitle { get; set; }
        public long? ProgramLevelId { get; set; }
        public long? StreamId { get; set; }
        public long? SpecializationId { get; set; }
        public long? ProgramDurationId { get; set; }
        public string? ProgramDurationTitle { get; set; }
        public string? ProgramCampus { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public long? QualificationLevelId { get; set; }
        public long? TypeOfGradeId { get; set; }
        public string? ScoresRequired { get; set; }
        public decimal? TotalAnnualFees { get; set; }
        public string? MaxBacklogAllowed { get; set; }
        public bool IsIELTSRequired { get; set; }
        public bool IsTOEFLRequired { get; set; }
        public bool IsGRERequired { get; set; }
        public bool IsGMATRequired { get; set; }
        public bool IsPTERequired { get; set; }
        public bool IsOtherScoreRequired { get; set; }
        public string? IELTSListening { get; set; }
        public string? IELTSReading { get; set; }
        public string? IELTSWriting { get; set; }
        public string? IELTSSpeaking { get; set; }
        public string? IELTSOverAll { get; set; }
        public bool IsIELTSRelax { get; set; }
        public string? IELTSModules { get; set; }
        public string? IELTSScore { get; set; }
        public string? PopulateInstituteAddress { get; set; }
        public string? ProgramURL { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public string StatusFlag { get; set; } = "A";
        public string StatusDisplay => StatusFlag == "A" ? "Active" : "InActive";
    }
}
