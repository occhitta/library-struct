using Occhitta.Libraries.Common;

namespace Occhitta.Libraries.Stream.String;

/// <summary>
/// <see cref="SimpleStringLightReader" />検証クラスです。
/// </summary>
[TestFixture]
public class SimpleStringLightReaderTest : StringLightReaderTest {
	#region 継承メソッド定義:InvokeTest/UpdateName
	/// <summary>
	/// 引数情報を実行します。
	/// </summary>
	/// <param name="source">要素内容</param>
	/// <param name="action">検証処理</param>
	protected override void InvokeTest(string source, Action<StringLightReader> action) {
		using var reader = new SimpleStringLightReader(source);
		action(reader);
	}
	/// <summary>
	/// 要素名称を置換します。
	/// </summary>
	/// <param name="source">要素名称</param>
	/// <returns>置換内容</returns>
	protected override string UpdateName(string source) => source switch {
		"ObjectRoot" => "Occhitta.Libraries.Struct",
		"ObjectName" => "Occhitta.Libraries.Stream.String.SimpleStringLightReader",
		"ObjectCode" => "System.String get_Source()",
		"SourceName" => "Occhitta.Libraries.Struct",
		"BufferName" => "buffer",
		"BufferCode" => "(System.Reflection.RuntimeMethodInfo)Int32 Read(Char[], Int32, Int32)",
		"OffsetName" => "offset",
		"OffsetCode" => "(System.Reflection.RuntimeMethodInfo)Int32 Read(Char[], Int32, Int32)",
		"LengthName" => "length",
		"LengthCode" => "(System.Reflection.RuntimeMethodInfo)Int32 Read(Char[], Int32, Int32)",
		"BorderName" => "Null",
		"BorderCode" => "(System.Reflection.RuntimeMethodInfo)Int32 Read(Char[], Int32, Int32)",
		_            => base.UpdateName(source)
	};
	#endregion 継承メソッド定義:InvokeTest/UpdateName

	#region 検証メソッド定義:CreateTest
	/// <summary>
	/// 生成例外を検証します。
	/// </summary>
	/// <param name="source">読込情報</param>
	/// <returns>例外内容</returns>
	[TestCase(null, ExpectedResult = """
System.ArgumentNullException {
	Message : "Value cannot be null. (Parameter 'source')"
	ParamName : "source"
	TargetSite : (System.Reflection.RuntimeConstructorInfo)Void .ctor(System.String)
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2147467261
	StackTrace : ...
}
""")]
	public string CreateTest(string source) {
		var result = Assert.Throws<ArgumentNullException>(() => new SimpleStringLightReader(source));
		return ExceptionUtilities.ToString(result, "\t");
	}
	#endregion 検証メソッド定義:CreateTest
}
