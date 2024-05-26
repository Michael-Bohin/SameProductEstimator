global using static System.Console;
using SameProductEstimator;
using SameProductEstimator.Kosik;
using SameProductEstimator.Rohlik;
using SameProductEstimator.Tesco;
using System.IO.Compression;
using System.Text;

const string kosikProductDataRelativePath = "./../../../ProductParser/ScrapedEshopData/kosikProductDataIndented.json";

string json = FileLoader.LoadFileContents(kosikProductDataRelativePath);

KosikAdapter ka = new();

List<NormalizedProduct> kosikProducts = ka.GetNormalizedProducts(json);	

ProductParserLogger.Log(kosikProducts, Eshop.Kosik);



const string rohlikZipesRelativePath = "./../../../ProductParser/ScrapedEshopData/rohlikProductData.zip";
string extractPath = @"./out/decompressedFiles/";
if (Directory.Exists(extractPath))
	Directory.Delete(extractPath, true);
Directory.CreateDirectory(extractPath);

ZipFile.ExtractToDirectory(rohlikZipesRelativePath, extractPath);

string rohlikExtractedRelativeFilePath = $"{extractPath}rohlikProductData.json";

json = File.ReadAllText(rohlikExtractedRelativeFilePath, Encoding.UTF8);

Directory.Delete(extractPath, true); // clean up unziped file

RohlikAdapter ra = new();

List<NormalizedProduct> rohlikProducts = ra.GetNormalizedProducts(json);

ProductParserLogger.Log(rohlikProducts, Eshop.Rohlik);


const string tescoProductDataRelativePath = "./../../../ProductParser/ScrapedEshopData/tescoProductData.json";

json = FileLoader.LoadFileContents(tescoProductDataRelativePath);

TescoAdapter ta = new();

List<NormalizedProduct> tescoProducts = ta.GetNormalizedProducts(json);

ProductParserLogger.Log(tescoProducts, Eshop.Tesco);
