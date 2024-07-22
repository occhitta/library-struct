using System.Text.RegularExpressions;
using Occhitta.Libraries.Common;

namespace Occhitta.Libraries.Stream;

/// <summary>
/// <see cref="StringLightReader" />検証クラスです。
/// </summary>
public abstract partial class StringLightReaderTest {
	#region メンバー定数定義
	/// <summary>書式情報(ArgumentNullException用)</summary>
	private const string Template1 = """
System.ArgumentNullException {
	Message : "Value cannot be null. (Parameter '${Argument:SourceName}')"
	ParamName : "${Argument:SourceName}"
	TargetSite : ${Argument:TargetSite}
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "${Template:SourceName}"
	HResult : -2147467261
	StackTrace : ...
}
""";
	/// <summary>書式情報(ArgumentOutOfRangeException用)</summary>
	private const string Template2 = """
System.ArgumentOutOfRangeException {
	Message : "${Argument:SourceName} ('-1') must be a non-negative value. (Parameter '${Argument:SourceName}')\r\nActual value was -1."
	ActualValue : -1
	ParamName : "${Argument:SourceName}"
	TargetSite : ${Argument:TargetSite}
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "${Template:SourceName}"
	HResult : -2146233086
	StackTrace : ...
}
""";
	/// <summary>書式情報(ArgumentException用)</summary>
	private const string Template3 = """
System.ArgumentException {
	Message : "Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."
	ParamName : ${Argument:SourceName}
	TargetSite : ${Argument:TargetSite}
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "${Template:SourceName}"
	HResult : -2147024809
	StackTrace : ...
}
""";
	/// <summary>書式情報(ObjectDisposedException用)</summary>
	private const string Template4 = """
System.ObjectDisposedException {
	Message : "Cannot access a disposed object.\r\nObject name: '${Template:ObjectName}'."
	ObjectName : "${Template:ObjectName}"
	TargetSite : (System.Reflection.RuntimeMethodInfo)${Template:ObjectCode}
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "${Template:ObjectRoot}"
	HResult : -2146232798
	StackTrace : ...
}
""";
	#endregion メンバー定数定義

	#region メンバー変数定義
	/// <summary>要素辞書</summary>
	private readonly Dictionary<string, string> elements = [];
	#endregion メンバー変数定義

	#region 内部メソッド定義:CreateCode/CreateData/UpdateName/UpdateText
	/// <summary>
	/// 正規表現を取得します。
	/// </summary>
	/// <returns>正規表現</returns>
	[GeneratedRegex(@"\$\{(Template|Argument)\:([A-Za-z]+)\}")]
	private static partial Regex CreateCode();
	/// <summary>
	/// 要素名称を置換します。
	/// </summary>
	/// <param name="source">要素名称</param>
	/// <returns>置換内容</returns>
	protected virtual string UpdateName(string source) => source;
	/// <summary>
	/// 要素情報を置換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <returns>置換内容</returns>
	private string UpdateName(Match source) {
		var cache1 = source.Groups[1];
		var cache2 = source.Groups[2];
		return cache1.Value switch {
			"Template" => UpdateName(cache2.Value),
			"Argument" => UpdateName(this.elements[cache2.Value]),
			_          => UpdateName(cache2.Value),
		};
	}
	/// <summary>
	/// 要素内容を置換します。
	/// </summary>
	/// <param name="source">要素内容</param>
	/// <returns>置換内容</returns>
	private string UpdateText(string source) {
		var choose = CreateCode();
		return choose.Replace(source, UpdateName);
	}
	#endregion 内部メソッド定義:CreateCode/CreateData/UpdateName/UpdateText

	#region 検証メソッド定義:InvokeTest
	/// <summary>
	/// 引数情報を実行します。
	/// </summary>
	/// <param name="source">要素内容</param>
	/// <param name="action">検証処理</param>
	protected abstract void InvokeTest(string source, Action<StringLightReader> action);
	#endregion 検証メソッド定義:InvokeTest

	#region 検証メソッド定義:StringLightReader
	/// <summary>
	/// 成功処理を検証します。
	/// <para>以下のメソッドを検証します。
	///   <para><see cref="StringLightReader.Read()" /></para>
	/// </para>
	/// </summary>
	[Test]
	public void SuccessA1() {
		InvokeTest("TEST", reader => {
			Assert.That(reader.Read(), Is.EqualTo('T'));
			Assert.That(reader.Read(), Is.EqualTo('E'));
			Assert.That(reader.Read(), Is.EqualTo('S'));
			Assert.That(reader.Read(), Is.EqualTo('T'));
			Assert.That(reader.Read(), Is.EqualTo( -1));
			Assert.That(reader.Read(), Is.EqualTo( -1));
		});
	}
	/// <summary>
	/// 成功処理を検証します。
	/// <para>以下のメソッドを検証します。
	///   <para><see cref="StringLightReader.Read(char[])" /></para>
	///   <para><see cref="StringLightReader.Read(char[], int, int)" /></para>
	/// </para>
	/// </summary>
	[Test]
	public void SuccessB1() {
		var buffer = new char[3];
		InvokeTest("TEST", reader => {
			Assert.That(reader.Read(buffer), Is.EqualTo(3));
			Assert.Multiple(() => {
				Assert.That(buffer[0], Is.EqualTo('T'));
				Assert.That(buffer[1], Is.EqualTo('E'));
				Assert.That(buffer[2], Is.EqualTo('S'));
			});
		});
	}
	/// <summary>
	/// 成功処理を検証します。
	/// <para>以下のメソッドを検証します。
	///   <para><see cref="StringLightReader.Read(char[])" /></para>
	///   <para><see cref="StringLightReader.Read(char[], int, int)" /></para>
	/// </para>
	/// </summary>
	[Test]
	public void SuccessB2() {
		var buffer = new char[6];
		for (var index = 0; index < buffer.Length; index ++) {
			buffer[index] = (char)index;
		}
		InvokeTest("TEST", reader => {
			Assert.That(reader.Read(buffer), Is.EqualTo(4));
			Assert.Multiple(() => {
				Assert.That(buffer[0], Is.EqualTo('T'));
				Assert.That(buffer[1], Is.EqualTo('E'));
				Assert.That(buffer[2], Is.EqualTo('S'));
				Assert.That(buffer[3], Is.EqualTo('T'));
				Assert.That(buffer[4], Is.EqualTo(004));
				Assert.That(buffer[5], Is.EqualTo(005));
			});
		});
	}
	/// <summary>
	/// 成功処理を検証します。
	/// <para>以下のメソッドを検証します。
	///   <para><see cref="StringLightReader.Read(char[])" /></para>
	///   <para><see cref="StringLightReader.Read(char[], int, int)" /></para>
	/// </para>
	/// </summary>
	/// <remarks>保存配列が「空」であっても例外は発生しない。
	/// この場合、実装によっていは永久ループになる。</remarks>
	[Test]
	public void SuccessB3() {
		var buffer = Array.Empty<char>();
		InvokeTest("TEST", reader => {
			Assert.That(reader.Read(buffer), Is.EqualTo(0));
		});
	}
	/// <summary>
	/// 失敗処理を検証します。
	/// <para>以下のメソッドを検証します。
	///   <para><see cref="StringLightReader.Read(char[], int, int)" /></para>
	/// </para>
	/// </summary>
	/// <param name="buffer">要素個数</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <param name="expect">想定書式</param>
	/// <param name="param1">引数名称</param>
	/// <param name="param2">引数名称</param>
	[TestCase(-1, +0, +0, Template1, "BufferName", "BufferCode")]
	[TestCase(+3, -1, +1, Template2, "OffsetName", "OffsetCode")]
	[TestCase(+3, +1, -1, Template2, "LengthName", "LengthCode")]
	[TestCase(+3, +1, +3, Template3, "BorderName", "BorderCode")]
	[TestCase(+3, +3, +1, Template3, "BorderName", "BorderCode")]
	public void FailureB1(int buffer, int offset, int length, string expect, string param1, string param2) {
		InvokeTest("TEST", reader => {
			var caches = buffer < 0? null: new char[buffer];
			#pragma warning disable CS8604 // Null参照抑制
			var result = Assert.Catch<Exception>(() => reader.Read(caches, offset, length));
			#pragma warning restore CS8604 // Null参照抑制
			this.elements["SourceName"] = param1;
			this.elements["TargetSite"] = param2;
			Assert.That(ExceptionUtilities.ToString(result, "\t"), Is.EqualTo(UpdateText(expect)));
		});
	}
	#endregion 検証メソッド定義:StringLightReader

	#region 検証メソッド定義:IDisposable
	/// <summary>
	/// <see cref="IDisposable.Dispose" />を検証します。
	/// </summary>
	[Test]
	public void DisposeA1() {
		InvokeTest("", reader => {
			reader.Dispose();
			var result = Assert.Throws<ObjectDisposedException>(() => reader.Read());
			Assert.That(ExceptionUtilities.ToString(result, "\t"), Is.EqualTo(UpdateText(Template4)));
		});
	}
	#endregion 検証メソッド定義:IDisposable
}
