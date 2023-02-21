public class Attributes
{
    public double LineHeight { get; set; }
    public double SpaceAfter { get; set; }
    public string? TextAlign { get; set; }
    public string? Placement { get; set; }
}

public class Boxes
{
    public List<double>? CropBox { get; set; }
    public List<double>? MediaBox { get; set; }
}

public class Element
{
    public List<double>? Bounds { get; set; }
    public List<double>? ClipBounds { get; set; }
    public Font? Font { get; set; }
    public bool HasClip { get; set; }
    public string? Lang { get; set; }
    public int Page { get; set; }
    public string? Path { get; set; }
    public string? Text { get; set; }
    public double TextSize { get; set; }
    public Attributes? attributes { get; set; }
}

public class ExtendedMetadata
{
    public string? ID_instance { get; set; }
    public string? ID_permanent { get; set; }
    public string? pdf_version { get; set; }
    public string? pdfa_compliance_level { get; set; }
    public bool is_encrypted { get; set; }
    public bool has_acroform { get; set; }
    public bool is_digitally_signed { get; set; }
    public string? pdfua_compliance_level { get; set; }
    public double page_count { get; set; }
    public bool has_embedded_files { get; set; }
    public bool is_certified { get; set; }
    public bool is_XFA { get; set; }
    public string? language { get; set; }
}

public class Font
{
    public string? alt_family_name { get; set; }
    public bool embedded { get; set; }
    public string? encoding { get; set; }
    public string? family_name { get; set; }
    public string? font_type { get; set; }
    public bool italic { get; set; }
    public bool monospaced { get; set; }
    public string? name { get; set; }
    public bool subset { get; set; }
    public double weight { get; set; }
}

public class Page
{
    public Boxes? boxes { get; set; }
    public double height { get; set; }
    public bool is_scanned { get; set; }
    public double page_number { get; set; }
    public double rotation { get; set; }
    public double width { get; set; }
}

public class Root
{
    public Version? version { get; set; }
    public ExtendedMetadata extended_metadata { get; set; }
    public List<Element> elements { get; set; }
    public List<Page>? pages { get; set; }
}

public class Version
{
    public string? json_export { get; set; }
    public string? page_segmentation { get; set; }
    public string? schema { get; set; }
    public string? structure { get; set; }
    public string? table_structure { get; set; }
}

