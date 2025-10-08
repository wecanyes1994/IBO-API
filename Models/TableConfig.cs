// Models/TableConfig.cs
namespace IBOWebAPI.Models
{
    public sealed class TableConfig
    {
        public string TableName { get; init; } = string.Empty;
        public string KeyColumn { get; init; } = string.Empty;
        public string[] Columns { get; init; } = System.Array.Empty<string>();
        public string[]? SearchColumns { get; init; }
        public string? DefaultOrderBy { get; init; }
        public string[]? DateTimeColumns { get; init; }
    }

    public static class TableRegistry
    {
        public static readonly Dictionary<string, TableConfig> Tables =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["bas110"] = new TableConfig
                {
                    TableName = "dbo.bas110",
                    KeyColumn = "projcode",
                    Columns = new[] {
                        "projcode","projname","offcode","compcode",
                        "bc11001","bc11002","bc11003","bc11004","bc11005","bc11006",
                        "auditusr","auditdate","memo","status","crtdate","crtusr","moddate","modusr"
                    },
                    SearchColumns = new[] { "projcode", "projname" },
                    DefaultOrderBy = "crtdate DESC",
                    DateTimeColumns = new[] { "auditdate", "crtdate", "moddate" }
                },

                ["bas010"] = new TableConfig
                {
                    TableName = "dbo.bas010",
                    KeyColumn = "usrcode",
                    Columns = new[] {
                        "usrcode","usrname","offcode","dptcode","usrpwd",
                        "bc01001","bc01002","bc01003","bc01004","bc01005",
                        "bc01006","bc01007","bc01008","bc01009","bc01010",
                        "bc01011","bc01012","bc01013","bc01014","bc01015",
                        "bc01016","bc01017","bc01018","bc01019","bc01020",
                        "memo","status","crtdate","crtusr","moddate","modusr"
                    },
                    SearchColumns = new[] { "usrcode", "usrname" },
                    DefaultOrderBy = "usrcode",
                    DateTimeColumns = new[] { "crtdate", "moddate" }
                },

                ["bas030"] = new TableConfig
                {
                    TableName = "dbo.bas030",
                    KeyColumn = "concode",
                    Columns = new[] {
                        "concode","conname",
                        "bc03001","bc03002","bc03003","bc03004","bc03005","bc03006","bc03007",
                        "bc03008","bc03009","bc03010","bc03011","bc03012","bc03013","bc03014",
                        "auditusr","auditdate","memo","status","crtdate","crtusr","moddate","modusr"
                    },
                    SearchColumns = new[] { "concode", "conname" },
                    DefaultOrderBy = "concode",
                    DateTimeColumns = new[] { "auditdate", "crtdate", "moddate" }
                },

                ["bas040"] = new TableConfig
                {
                    TableName = "dbo.bas040",
                    KeyColumn = "unitcode",
                    Columns = new[] {
                        "unitcode","unitname","unitsort","memo","status","crtdate","crtusr","moddate","modusr"
                    },
                    SearchColumns = new[] { "unitcode", "unitname" },
                    DefaultOrderBy = "unitcode",
                    DateTimeColumns = new[] { "crtdate", "moddate" }
                },

                ["bas050"] = new TableConfig
                {
                    TableName = "dbo.bas050",
                    KeyColumn = "compcode",
                    Columns = new[] {
                        "compcode","compname",
                        "comptype1","comptype2","comptype3","comptype4","comptype5","comptype6","comptype7",
                        "complv",
                        "bc05001","bc05002","bc05003","bc05004","bc05005","bc05006","bc05007","bc05008","bc05009","bc05010",
                        "bc05011","bc05012","bc05013","bc05014","bc05015","bc05016","bc05017","bc05018","bc05019","bc05020",
                        "bc05021","bc05022",
                        "auditusr","auditdate","memo","status","crtdate","crtusr","moddate","modusr"
                    },
                    SearchColumns = new[] { "compcode", "compname" },
                    DefaultOrderBy = "compcode",
                    DateTimeColumns = new[] { "auditdate", "crtdate", "moddate" }
                },

                ["bas060"] = new TableConfig
                {
                    TableName = "dbo.bas060",
                    KeyColumn = "sercode",
                    Columns = new[] {
                        "sercode","sername","compcode","unitcode",
                        "bc06001","bc06002","bc06003","bc06004","bc06005","bc06006","bc06007","bc06008",
                        "bc06009","bc06010","bc06011","bc06012","bc06013","bc06014","bc06015",
                        "memo","status","crtdate","crtusr","moddate","modusr"
                    },
                    SearchColumns = new[] { "sercode", "sername" },
                    DefaultOrderBy = "sercode",
                    DateTimeColumns = new[] { "crtdate", "moddate" }
                },

                ["bas061"] = new TableConfig
                {
                    TableName = "dbo.bas061",
                    // 若實際為複合鍵 (goodcode+sercode) 告訴我，我幫你升級成複合鍵版
                    KeyColumn = "goodcode",
                    Columns = new[] {
                        "goodcode","goodname","sercode",
                        "bc06101","bc06102","bc06103","bc06104","bc06105","bc06106",
                        "memo","status","crtdate","crtusr","moddate","modusr"
                    },
                    SearchColumns = new[] { "goodcode", "goodname" },
                    DefaultOrderBy = "goodcode",
                    DateTimeColumns = new[] { "crtdate", "moddate" }
                },

                ["sal020"] = new TableConfig
                {
                    TableName = "dbo.sal020",
                    KeyColumn = "resecode",
                    Columns = new[] {
                        "resecode","projcode","offcode","quotcode",
                        "sc02001","sc02002","sc02003","sc02004","sc02005","sc02006","sc02007",
                        "sc02008","sc02009","sc02010","sc02011","sc02012","sc02013","sc02014",
                        "auditusr1","auditdate1","auditusr2","auditdate2",
                        "memo","status","crtdate","crtusr","moddate","modusr"
                    },
                    SearchColumns = new[] { "resecode", "projcode", "quotcode" },
                    DefaultOrderBy = "crtdate DESC",
                    DateTimeColumns = new[] { "auditdate1", "auditdate2", "crtdate", "moddate" }
                },

                ["sal021"] = new TableConfig
                {
                    TableName = "dbo.sal021",
                    KeyColumn = "serno",
                    Columns = new[] {
                        "serno","resecode","crec","goodcode",
                        "amt1","mny","unitcode","mnytotal1",
                        "sc02101","sc02102","sc02103",
                        "memo","crtdate","crtusr","moddate","modusr"
                    },
                    SearchColumns = new[] { "serno", "resecode", "goodcode" },
                    DefaultOrderBy = "serno",
                    DateTimeColumns = new[] { "crtdate", "moddate" }
                },

                ["sal022"] = new TableConfig
                {
                    TableName = "dbo.sal022",
                    KeyColumn = "serno",
                    Columns = new[] {
                        "serno","fromserno","goodcode","dimension","compcode",
                        "amt0","amt1","amt2","mny","unitcode","unitcode2","mnytotal",
                        "memo","crtdate","crtusr","moddate","modusr"
                    },
                    SearchColumns = new[] { "serno", "goodcode", "compcode" },
                    DefaultOrderBy = "serno",
                    DateTimeColumns = new[] { "crtdate", "moddate" }
                },

                ["sal023"] = new TableConfig
                {
                    TableName = "dbo.sal023",
                    KeyColumn = "serno",
                    Columns = new[] {
                        "serno","fromserno","goodcode","compcode","compuser",
                        "amt","mny","unitcode","mnytotal",
                        "memo","crtdate","crtusr","moddate","modusr"
                    },
                    SearchColumns = new[] { "serno", "goodcode", "compcode" },
                    DefaultOrderBy = "serno",
                    DateTimeColumns = new[] { "crtdate", "moddate" }
                },
            };
    }
}
