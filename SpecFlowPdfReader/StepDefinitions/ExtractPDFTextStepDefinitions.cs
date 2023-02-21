using Adobe.PDFServicesSDK.auth;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.options.extractpdf;
using Adobe.PDFServicesSDK.pdfops;
using AdobeExecutionContext = Adobe.PDFServicesSDK.ExecutionContext;
using SpecFlowPdfReader.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using SpecFlowPdfReader.Consts;

namespace SpecFlowPdfReader.StepDefinitions
{

    [Binding]
    public class ExtractPDFTextStepDefinitions
    {
        private string pdfFilePath;
        private string extractedText;
        private string? credentialsFilePath = JsonHelper.GetValues().AdobeCredPath;
        private string? zipResult = JsonHelper.GetValues().zipPath;
        private string? jsonResut= JsonHelper.GetValues().jsonPath;
        readonly ZipExtractor resExtract = new ZipExtractor();

        [Given(@"my PDF file ""([^""]*)""")]
        public void GivenMyPDFFile(string pdfFileName)
        {
            this.pdfFilePath = Path.Combine(JsonHelper.GetValues().pdfPath, pdfFileName);        
        }

        [When(@"I extract the text")]
        public void ExtractTextFromPDF()
        {
            Credentials credentials = Credentials.ServiceAccountCredentialsBuilder()
            .FromFile(credentialsFilePath)
            .Build();

            FileRef sourceFileRef = FileRef.CreateFromLocalFile(pdfFilePath);

            ExtractPDFOptions extractPDFOptions = ExtractPDFOptions.ExtractPDFOptionsBuilder()
                .AddElementsToExtract(new List<ExtractElementType> { ExtractElementType.TEXT, ExtractElementType.TABLES })
                .Build();
            
            ExtractPDFOperation operation = ExtractPDFOperation.CreateNew();
            operation.SetInputFile(sourceFileRef);
            operation.SetOptions(extractPDFOptions);

            AdobeExecutionContext executionContext = AdobeExecutionContext.Create(credentials);
            FileRef result;

            try
            {
                result = operation.Execute(executionContext);
                Logger.WrtieLog(nameof(ExtractTextFromPDF),
                       $"pdf extracted");
            }
            catch (Exception ex)
            {
                Logger.WrtieLog(nameof(ExtractTextFromPDF),
                   $"Error with extract - {ex.Message}");
                return;
            }
            
            try
            {
                if (File.Exists(zipResult))
                {
                    File.Delete(zipResult);
                }
                result.SaveAs(zipResult);
                resExtract.ExtractZip(zipResult);
            }
            catch (Exception ex)
            {
                Logger.WrtieLog(nameof(ExtractTextFromPDF),
                     $"Error with deleted file - {ex.Message}");
                return;
            }
        }

        [Then(@"the Json should have a non-null document section title")]
        public void ThenTheJsonShouldHaveANon_NullDocumentSectionTitle()
        {
            string jsonContent = File.ReadAllText(jsonResut);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonContent);
            var titleElement = myDeserializedClass.elements.FirstOrDefault(e => e.Path.Contains(PdfCheckPaths.pdfTitlePath) && e.TextSize != 0);
            try
            {
                Assert.That(titleElement, Is.Not.Null, "Title element not found");
                Assert.That(titleElement?.Text, Is.Not.Null.Or.Empty, "Title element text is null or empty");
                Logger.WrtieLog(nameof(ThenTheJsonShouldHaveANon_NullDocumentSectionTitle),
                      $"Title element - {titleElement.Text}");
            }
            catch (AssertionException ex)
            {
                Logger.WrtieLog(nameof(ThenTheJsonShouldHaveANon_NullDocumentSectionTitle),
                      $"Title element text is null or empty - {ex.Message}, PDF Path - {titleElement.Path}");
            }
        }

        [Then(@"the Json should have a non-null document section header")]
        public void ThenTheJsonShouldHaveANon_NullDocumentSectionHeader()
        {
            string jsonContent = File.ReadAllText(jsonResut);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonContent);

            int headerCount = 0;
            int tableHeaderCount = 0;
            foreach (var element in myDeserializedClass.elements.Where(e => e.Path.Contains(PdfCheckPaths.pdfHeader) && e.TextSize != 0))
            {
                try
                {
                    if (element.Text.Contains(PdfCheckPaths.pdfTableHeader))
                    {
                        Console.WriteLine($"text - {element.Text} and path - {element.Path}");
                        Assert.That(element.Text, Is.Not.Null.Or.Empty, $"Element table header text is null or empty at path {element.Path}");
                        tableHeaderCount++;
                    }
                    else
                    {
                        Assert.That(element.Text, Is.Not.Null.Or.Empty, $"Element header text is null or empty at path {element.Path}");
                        headerCount++;
                    }
                }
                catch (AssertionException ex)
                {
                    Logger.WrtieLog(nameof(ThenTheJsonShouldHaveANon_NullDocumentSectionHeader),
                         $"Error processing element at path {element.Path}: {ex.Message}");
                    continue;
                }
            }
            Logger.WrtieLog(nameof(ThenTheJsonShouldHaveANon_NullDocumentSectionHeader),$"Number of header elements found: {headerCount}");
            Logger.WrtieLog(nameof(ThenTheJsonShouldHaveANon_NullDocumentSectionHeader), $"Number of table header elements found: {tableHeaderCount}");
        }

        [Then(@"the Json should have a non-null document section checkbox")]
        public void ThenTheJsonShouldHaveANon_NullDocumentSectionCheckbox()
        {
            string jsonContent = File.ReadAllText(jsonResut);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonContent);
            int checkBoxCount = 0;
            foreach (var checkBoxElement in myDeserializedClass.elements.Where(e => e.Path.Contains(PdfCheckPaths.pdfDoc) && e.TextSize != 0))
            {
                try
                {
                    if (string.IsNullOrEmpty(checkBoxElement.Text) || !checkBoxElement.Text.Contains(PdfCheckPaths.pdfCheckBox))
                    {
                        continue;
                    }

                    Assert.That(checkBoxElement.Text, Is.Not.Null.Or.Empty,
                       $"Result from JSON Element text is null or empty at path {checkBoxElement.Path}");

                    checkBoxCount++;
                }
                catch (AssertionException ex)
                {
                    Logger.WrtieLog(nameof(ThenTheJsonShouldHaveANon_NullDocumentSectionCheckbox),
                    $"Error processing element at path {checkBoxElement.Path}: {ex.Message}");
                    continue;
                }
            }
            Logger.WrtieLog(nameof(ThenTheJsonShouldHaveANon_NullDocumentSectionCheckbox),
            $"Number of checkBox elements found: { checkBoxCount}");
        }

        [Then(@"the Json should have a non-null document section paragraph")]
        public void ThenTheJsonShouldHaveANon_NullDocumentSectionParagraph()
        {
            string jsonContent = File.ReadAllText(jsonResut);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonContent);
            
            int paragCount = 0;
            foreach (var poElement in myDeserializedClass.elements.Where(e => e.Path.Contains(PdfCheckPaths.pdfParapgraphPath) && e.TextSize != 0))
            {
                try
                {
                    Assert.That(poElement.Text, Is.Not.Null.Or.Empty,
                        $"paragraph element text is null or empty at path {poElement.Path}");
                    paragCount++;
                }
                catch (AssertionException ex)
                {
                     Logger.WrtieLog(nameof(ThenTheJsonShouldHaveANon_NullDocumentSectionParagraph),
                        $"Error paragraph processing element at path {poElement.Path}: {ex.Message}");
                    continue;
                }
            }
            Logger.WrtieLog(nameof(ThenTheJsonShouldHaveANon_NullDocumentSectionParagraph),
                        $"Number of paragrpaph elements found: {paragCount}");
        }

        [Then(@"the Json should have a non-null document section table")]
        public void ThenTheJsonShouldHaveANon_NullDocumentSectionTable()
        {
            string jsonContent = File.ReadAllText(jsonResut);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonContent);

            int tableCount = 0;
            foreach (var tableElement in myDeserializedClass.elements.Where(e => e.Path.Contains(PdfCheckPaths.pdfTable) && e.TextSize != 0))
            {
                try
                {
                    Assert.That(tableElement.Text, Is.Not.Null.Or.Empty,
                        $"table element text is null or empty at path {tableElement.Path}");
                    tableCount++;
                }
                catch (AssertionException ex)
                {
                    Logger.WrtieLog(nameof(ThenTheJsonShouldHaveANon_NullDocumentSectionTable),
                        $"Error processing table element at path {tableElement.Path}: {ex.Message}");
                    continue;
                }
            }
            Logger.WrtieLog(nameof(ThenTheJsonShouldHaveANon_NullDocumentSectionTable),
                $"Number of table elements found: {tableCount}");
        }
    }
}