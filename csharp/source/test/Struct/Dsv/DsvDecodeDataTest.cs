using System.Reflection;
using Occhitta.Libraries.Common;

namespace Occhitta.Libraries.Struct.Dsv;

/// <summary>
/// <see cref="DsvDecodeData{TParse, TStore}" />検証クラスです。
/// </summary>
[TestFixture]
public class DsvDecodeDataTest {
	#region 内部メソッド定義:Convert
	/// <summary>
	/// 要素一覧へ変換します。
	/// </summary>
	/// <typeparam name="TValue">要素種別</typeparam>
	/// <param name="sourceList">要素集合</param>
	/// <returns>要素一覧</returns>
	private static List<TValue> Convert<TValue>(IEnumerable<TValue> sourceList) =>
		new(sourceList);
	#endregion 内部メソッド定義:Convert

	#region 内部メソッド定義:Execute
	/// <summary>
	/// 引数情報を検証します。
	/// </summary>
	/// <typeparam name="TParse">項目種別</typeparam>
	/// <typeparam name="TStore">要素種別</typeparam>
	/// <param name="exportHook">出力処理</param>
	/// <param name="escapeCode">制御文字</param>
	/// <param name="markerCode">区切文字</param>
	/// <param name="importList">取込一覧</param>
	/// <param name="finishData">終了情報</param>
	private static void Execute<TParse, TStore>(DsvResultCode<TParse, TStore> exportHook, char escapeCode, char markerCode, ImportData<TParse, TStore>[] importList, FinishData<TParse, TStore> finishData) {
		var parserCode = new ParserCode<TParse>();
		using var sourceData = new DsvDecodeData<TParse, TStore>(parserCode.InvokeTest, exportHook, escapeCode, markerCode);
		foreach (var importData in importList) {
			importData.InvokeTest(sourceData, parserCode);
		}
		finishData.InvokeTest(sourceData, parserCode);
	}
	/// <summary>
	/// 引数情報を検証します。
	/// </summary>
	/// <param name="escapeCode">制御文字</param>
	/// <param name="markerCode">区切文字</param>
	/// <param name="importList">取込一覧</param>
	/// <param name="finishData">終了情報</param>
	private static void Execute(char escapeCode, char markerCode, ImportData<string, List<string>>[] importList, FinishData<string, List<string>> finishData) =>
		Execute(Convert, escapeCode, markerCode, importList, finishData);
	#endregion 内部メソッド定義:Execute

	#region 検証メソッド定義:Success1
	/// <summary>
	/// 正常処理を検証します。
	/// </summary>
	[Test]
	public void Success1() {
		Execute('"', ',', [
			new('"', null, 0, false, null),
			new('A', null, 0, false, null),
			new('"', null, 0, false, null),
			new(',', new("\"A\"", 0, 0, "A"), 1, false, null),
			new('"', null, 1, false, null),
			new('B', null, 1, false, null),
			new('"', null, 1, false, null),
		], new(new("\"B\"", 1, 0, "B"), 2, true, new(["A", "B"])));
		Execute('"', ',', [
			new('"',  null,                    0, false, null),
			new('A',  null,                    0, false, null),
			new('"',  null,                    0, false, null),
			new(',',  new("\"A\"", 0, 0, "A"), 1, false, null),
			new('\r', null,                    1, false, null),
			new('\n', new("",      1, 0, "" ), 2, true,  new(["A", ""])),
			new('"',  null,                    2, false, null),
			new('B',  null,                    2, false, null),
			new('"',  null,                    2, false, null),
			new(',',  new("\"B\"", 0, 1, "B"), 3, false, null)
		],  new(      new("",      1, 1, "" ), 4, true,  new(["B", ""])));
		Execute('"', ',', [
		],  new(      null,                    0, false, null));
	}
	#endregion 検証メソッド定義:Success1

	#region 検証メソッド定義:Failure1
	[Test(ExpectedResult = """
Occhitta.Libraries.Struct.StructException {
	Reason : "DSV項目の文字列が終了していません。"
	Region : "\""
	Offset : 0
	Length : 1
	TargetSite : (System.Reflection.RuntimeMethodInfo)TParse DecodeItem(Occhitta.Libraries.Struct.Dsv.DsvParserCode`1[TParse], System.Text.StringBuilder, Int32, Int32)
	Message : "DSV項目の文字列が終了していません。\r\nOffset : 0\r\nLength : 1\r\nRegion : \"\r\n       : ^"
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2146233087
	StackTrace : ...
}
""")]
	/// <summary>
	/// 異常処理を検証します。
	/// </summary>
	public string Failure1() {
		var result = Assert.Throws<StructException>(() => Execute('"', ',', [new('"', null, 0, false, null)], new(null, 1, false, null)));
		return ExceptionUtilities.ToString(result, "\t");
	}
	#endregion 検証メソッド定義:Failure1

	#region 検証メソッド定義:Failure2
	[Test(ExpectedResult = """
Occhitta.Libraries.Struct.StructException {
	Reason : "DSV項目に単一のキャリッジリターンが存在します。"
	Region : "\r"
	Offset : 0
	Length : 1
	TargetSite : (System.Reflection.RuntimeMethodInfo)Boolean DecodeData(Occhitta.Libraries.Struct.Dsv.DsvResultCode`2[TParse,TStore], System.Collections.Generic.List`1[TParse], Occhitta.Libraries.Struct.Dsv.DsvParserCode`1[TParse], System.Text.StringBuilder, Char, TStore ByRef)
	Message : "DSV項目に単一のキャリッジリターンが存在します。\r\nOffset : 0\r\nLength : 1\r\nRegion : \r\r\n       : ^"
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2146233087
	StackTrace : ...
}
""")]
	/// <summary>
	/// 異常処理を検証します。
	/// </summary>
	public string Failure2() {
		var result = Assert.Throws<StructException>(() => Execute('"', ',', [new('\r', null, 0, false, null), new(' ', null, 0, false, null)], new(null, 1, false, null)));
		return ExceptionUtilities.ToString(result, "\t");
	}
	#endregion 検証メソッド定義:Failure2



	/// <summary>
	/// カスタム例外を発行します。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="columnCode">情報番号</param>
	/// <param name="recordCode">要素番号</param>
	/// <returns>項目情報</returns>
	/// <exception cref="StructException">常に発行</exception>
	private static string Failure3(string sourceData, int columnCode, int recordCode) =>
		throw new StructException("カスタム例外", "*", 0, 1);
	/// <summary>
	/// 異常処理を検証します。
	/// </summary>
	[Test(ExpectedResult = """
Occhitta.Libraries.Struct.StructException {
	Reason : "カスタム例外"
	Region : "*"
	Offset : 0
	Length : 1
	TargetSite : (System.Reflection.RuntimeMethodInfo)System.String Failure3(System.String, Int32, Int32)
	Message : "カスタム例外\r\nOffset : 0\r\nLength : 1\r\nRegion : *\r\n       : ^"
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct.Test"
	HResult : -2146233087
	StackTrace : ...
}
""")]
	public string Failure3() {
		using var sourceData = new DsvDecodeData<string, List<string>>(Failure3, sourceList => new(sourceList), '"', ',');
		var result = Assert.Throws<StructException>(() => sourceData.ImportData(',', out var recordData));
		return ExceptionUtilities.ToString(result, "\t");
	}

	/// <summary>
	/// カスタム例外を発行します。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="columnCode">情報番号</param>
	/// <param name="recordCode">要素番号</param>
	/// <returns>項目情報</returns>
	/// <exception cref="StructException">常に発行</exception>
	private static string Failure4(string sourceData, int columnCode, int recordCode) =>
		throw new ArgumentException("引数例外", nameof(sourceData));
	/// <summary>
	/// 異常処理を検証します。
	/// </summary>
	[Test(ExpectedResult = """
Occhitta.Libraries.Struct.StructException {
	Reason : "DSV項目の解析に失敗しました。"
	Region : ""
	Offset : 0
	Length : 0
	TargetSite : (System.Reflection.RuntimeMethodInfo)TParse DecodeItem(Occhitta.Libraries.Struct.Dsv.DsvParserCode`1[TParse], System.Text.StringBuilder, Int32, Int32)
	Message : "DSV項目の解析に失敗しました。\r\nOffset : 0\r\nLength : 0\r\nRegion : \r\n       : "
	Data : {
	}
	InnerException : System.ArgumentException {
		Message : "引数例外 (Parameter 'sourceData')"
		ParamName : "sourceData"
		TargetSite : (System.Reflection.RuntimeMethodInfo)System.String Failure4(System.String, Int32, Int32)
		Data : {
		}
		InnerException : Null
		HelpLink : Null
		Source : "Occhitta.Libraries.Struct.Test"
		HResult : -2147024809
		StackTrace : ...
	}
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2146233087
	StackTrace : ...
}
""")]
	public string Failure4() {
		using var sourceData = new DsvDecodeData<string, List<string>>(Failure4, sourceList => new(sourceList), '"', ',');
		var result = Assert.Throws<StructException>(() => sourceData.ImportData(',', out var recordData));
		return ExceptionUtilities.ToString(result, "\t");
	}


	/// <summary>
	/// <see cref="IDisposable" />検証クラスです。
	/// </summary>
	[TestFixture]
	public class DisposeTest {
		#region 内部メソッド定義:Execute
		/// <summary>
		/// 復号処理を生成します。
		/// </summary>
		/// <typeparam name="TParse">項目種別</typeparam>
		/// <typeparam name="TStore">要素種別</typeparam>
		/// <param name="parserHook">解析処理</param>
		/// <param name="exportHook">出力処理</param>
		/// <returns>復号処理</returns>
		private static DsvDecodeData<TParse, TStore> Execute<TParse, TStore>(DsvParserCode<TParse> parserHook, DsvResultCode<TParse, TStore> exportHook) =>
			new(parserHook, exportHook, '"', ',');
		/// <summary>
		/// 復号処理を生成します。
		/// </summary>
		/// <param name="parserHook">解析処理</param>
		/// <param name="exportHook">出力処理</param>
		/// <returns>復号処理</returns>
		private static DsvDecodeData<ExportData, List<ExportData>> Execute() =>
			Execute(ExportData.Create, Convert);
		#endregion 内部メソッド定義:Execute

		#region 検証メソッド定義:Success1
		/// <summary>
		/// 正常処理を検証します。
		/// </summary>
		[Test]
		public void Success1() {
			using var source = Execute();
			Assert.DoesNotThrow(() => ((IDisposable)source).Dispose());
			Assert.DoesNotThrow(() => ((IDisposable)source).Dispose());
		}
		#endregion 検証メソッド定義:Success1

		#region 検証メソッド定義:Failure1
		/// <summary>
		/// 異常処理を検証します。
		/// </summary>
		/// <param name="parserHook">解析処理</param>
		/// <returns>例外内容</returns>
		[TestCase(null, ExpectedResult = """
System.ArgumentNullException {
	Message : "Value cannot be null. (Parameter 'parserHook')"
	ParamName : "parserHook"
	TargetSite : (System.Reflection.RuntimeConstructorInfo)Void .ctor(Occhitta.Libraries.Struct.Dsv.DsvParserCode`1[TParse], Occhitta.Libraries.Struct.Dsv.DsvResultCode`2[TParse,TStore], Char, Char)
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2147467261
	StackTrace : ...
}
""")]
		public string Failure1(DsvParserCode<ExportData> parserHook) {
			var result = Assert.Throws<ArgumentNullException>(() => Execute(parserHook, Convert));
			return ExceptionUtilities.ToString(result, "\t");
		}
		/// <summary>
		/// 異常処理を検証します。
		/// </summary>
		/// <param name="exportHook">解析処理</param>
		/// <returns>例外内容</returns>
		[TestCase(null, ExpectedResult = """
System.ArgumentNullException {
	Message : "Value cannot be null. (Parameter 'exportHook')"
	ParamName : "exportHook"
	TargetSite : (System.Reflection.RuntimeConstructorInfo)Void .ctor(Occhitta.Libraries.Struct.Dsv.DsvParserCode`1[TParse], Occhitta.Libraries.Struct.Dsv.DsvResultCode`2[TParse,TStore], Char, Char)
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2147467261
	StackTrace : ...
}
""")]
		public string Failure1(DsvResultCode<ExportData, List<ExportData>> exportHook) {
			var result = Assert.Throws<ArgumentNullException>(() => Execute(ExportData.Create, exportHook));
			return ExceptionUtilities.ToString(result, "\t");
		}
		#endregion 検証メソッド定義:Failure1

		#region 検証メソッド定義:Failure2
		/// <summary>
		/// 異常処理を検証します。
		/// </summary>
		/// <param name="memberName">開放名称</param>
		/// <returns>例外内容</returns>
		[TestCase("parserHook", ExpectedResult = """
System.ObjectDisposedException {
	Message : "Cannot access a disposed object.\r\nObject name: 'Occhitta.Libraries.Struct.Dsv.DsvDecodeData`2[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Collections.Generic.List`1[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'."
	ObjectName : "Occhitta.Libraries.Struct.Dsv.DsvDecodeData`2[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Collections.Generic.List`1[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
	TargetSite : (System.Reflection.RuntimeMethodInfo)Occhitta.Libraries.Struct.Dsv.DsvParserCode`1[TParse] get_ParserHook()
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2146232798
	StackTrace : ...
}
""")]
		[TestCase("exportHook", ExpectedResult = """
System.ObjectDisposedException {
	Message : "Cannot access a disposed object.\r\nObject name: 'Occhitta.Libraries.Struct.Dsv.DsvDecodeData`2[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Collections.Generic.List`1[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'."
	ObjectName : "Occhitta.Libraries.Struct.Dsv.DsvDecodeData`2[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Collections.Generic.List`1[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
	TargetSite : (System.Reflection.RuntimeMethodInfo)Occhitta.Libraries.Struct.Dsv.DsvResultCode`2[TParse,TStore] get_ExportHook()
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2146232798
	StackTrace : ...
}
""")]
		[TestCase("bufferData", ExpectedResult = """
System.ObjectDisposedException {
	Message : "Cannot access a disposed object.\r\nObject name: 'Occhitta.Libraries.Struct.Dsv.DsvDecodeData`2[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Collections.Generic.List`1[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'."
	ObjectName : "Occhitta.Libraries.Struct.Dsv.DsvDecodeData`2[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Collections.Generic.List`1[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
	TargetSite : (System.Reflection.RuntimeMethodInfo)System.Text.StringBuilder get_BufferData()
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2146232798
	StackTrace : ...
}
""")]
		[TestCase("exportList", ExpectedResult = """
System.ObjectDisposedException {
	Message : "Cannot access a disposed object.\r\nObject name: 'Occhitta.Libraries.Struct.Dsv.DsvDecodeData`2[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Collections.Generic.List`1[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'."
	ObjectName : "Occhitta.Libraries.Struct.Dsv.DsvDecodeData`2[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Collections.Generic.List`1[[Occhitta.Libraries.Struct.Dsv.DsvDecodeDataTest+ExportData, Occhitta.Libraries.Struct.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
	TargetSite : (System.Reflection.RuntimeMethodInfo)System.Collections.Generic.List`1[TParse] get_ExportList()
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2146232798
	StackTrace : ...
}
""")]
		public string Failure2(string memberName) {
			var sourceData = Execute();
			var sourceCode = sourceData.GetType();
			var memberData = sourceCode.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.That(memberData, Is.Not.Null);
			memberData.SetValue(sourceData, null);
			var resultData = Assert.Throws<ObjectDisposedException>(() => sourceData.ImportData(',', out var recordData));
			return ExceptionUtilities.ToString(resultData, "\t");
		}
		#endregion 検証メソッド定義:Failure2
	}

	#region 非公開クラス定義:ExportData
	/// <summary>
	/// 出力情報クラスです。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="columnCode">情報番号</param>
	/// <param name="recordCode">要素番号</param>
	public sealed class ExportData(string sourceData, int columnCode, int recordCode) : IEquatable<ExportData> {
		#region プロパティー定義
		/// <summary>
		/// 要素情報を取得します。
		/// </summary>
		/// <value>要素情報</value>
		public string SourceData {
			get;
		} = sourceData;
		/// <summary>
		/// 情報番号を取得します。
		/// </summary>
		/// <value>情報番号</value>
		public int ColumnCode {
			get;
		} = columnCode;
		/// <summary>
		/// 要素番号を取得します。
		/// </summary>
		/// <value>要素番号</value>
		public int RecordCode {
			get;
		} = recordCode;
		#endregion プロパティー定義

		#region 生成メソッド定義
		/// <summary>
		/// 出力情報を生成します。
		/// </summary>
		/// <param name="sourceData">要素情報</param>
		/// <param name="columnCode">情報番号</param>
		/// <param name="recordCode">要素番号</param>
		/// <returns>出力情報</returns>
		public static ExportData Create(string sourceData, int columnCode, int recordCode) =>
			new(sourceData, columnCode, recordCode);
		#endregion 生成メソッド定義

		#region 実装メソッド定義
		/// <summary>
		/// 当該情報と等価であるか判定します。
		/// </summary>
		/// <param name="some">判定情報</param>
		/// <returns>等価である場合、<c>True</c>を返却</returns>
		public bool Equals(ExportData? some) => some != null
			&& Equals(SourceData, some.SourceData)
			&& Equals(ColumnCode, some.ColumnCode)
			&& Equals(RecordCode, some.RecordCode);
		#endregion 実装メソッド定義

		#region 継承メソッド定義
		/// <summary>
		/// 当該情報と等価であるか判定します。
		/// </summary>
		/// <param name="some">判定情報</param>
		/// <returns>等価である場合、<c>True</c>を返却</returns>
		public override bool Equals(object? some) => Equals(some as ExportData);
		/// <summary>
		/// 当該情報のハッシュ値を算出します。
		/// </summary>
		/// <returns>ハッシュ値</returns>
		public override int GetHashCode() => Tuple.Create(SourceData, ColumnCode, RecordCode).GetHashCode();
		#endregion 継承メソッド定義
	}
	#endregion 非公開クラス定義:ExportData

	#region 非公開クラス定義:ParserData
	/// <summary>
	/// 解析結果クラスです。
	/// </summary>
	/// <typeparam name="TParse">項目種別</typeparam>
	/// <param name="sourceData">要素情報</param>
	/// <param name="columnCode">情報番号</param>
	/// <param name="recordCode">要素番号</param>
	/// <param name="returnData">返却情報</param>
	private sealed class ParserData<TParse>(string sourceData, int columnCode, int recordCode, TParse returnData) {
		#region プロパティー定義
		/// <summary>
		/// 要素情報を取得します。
		/// </summary>
		/// <value>要素情報</value>
		public string SourceData {
			get;
		} = sourceData;
		/// <summary>
		/// 情報番号を取得します。
		/// </summary>
		/// <value>情報番号</value>
		public int ColumnCode {
			get;
		} = columnCode;
		/// <summary>
		/// 要素番号を取得します。
		/// </summary>
		/// <value>要素番号</value>
		public int RecordCode {
			get;
		} = recordCode;
		/// <summary>
		/// 返却情報を取得します。
		/// </summary>
		/// <value>返却情報</value>
		public TParse ReturnData {
			get;
		} = returnData;
		#endregion プロパティー定義
	}
	#endregion 非公開クラス定義:ParserData

	#region 非公開クラス定義:ParserCode
	/// <summary>
	/// 解析処理クラスです。
	/// </summary>
	/// <typeparam name="TParse">項目種別</typeparam>
	private sealed class ParserCode<TParse> {
		#region メンバー変数定義
		/// <summary>解析処理</summary>
		private ParserData<TParse>? parserData = null;
		/// <summary>実行回数</summary>
		private int invokeSize = 0;
		#endregion メンバー変数定義

		#region プロパティー定義
		/// <summary>
		/// 解析処理を取得または設定します。
		/// </summary>
		/// <value>解析処理</value>
		public ParserData<TParse>? ParserData {
			get => this.parserData;
			set => this.parserData = value;
		}
		/// <summary>
		/// 実行回数を取得します。
		/// </summary>
		/// <value>実行回数</value>
		public int InvokeSize {
			get => this.invokeSize;
		}
		#endregion プロパティー定義

		#region 検証メソッド定義
		/// <summary>
		/// 引数情報を検証します。
		/// </summary>
		/// <param name="sourceData">要素情報</param>
		/// <param name="columnCode">情報番号</param>
		/// <param name="recordCode">要素番号</param>
		/// <returns>項目情報</returns>
		public TParse InvokeTest(string sourceData, int columnCode, int recordCode) {
			Assert.That(this.parserData, Is.Not.Null);
			Assert.Multiple(() => {
				Assert.That(sourceData, Is.EqualTo(this.parserData.SourceData));
				Assert.That(columnCode, Is.EqualTo(this.parserData.ColumnCode));
				Assert.That(recordCode, Is.EqualTo(this.parserData.RecordCode));
			});
			this.invokeSize ++;
			return this.parserData.ReturnData;
		}
		#endregion 検証メソッド定義
	}
	#endregion 非公開クラス定義:ParserCode

	#region 非公開クラス定義:ImportData
	/// <summary>
	/// 取込情報クラスです。
	/// </summary>
	/// <typeparam name="TParse">項目種別</typeparam>
	/// <typeparam name="TStore">要素種別</typeparam>
	/// <param name="importCode">取込文字</param>
	/// <param name="parserData">項目情報</param>
	/// <param name="invokeSize">実行回数</param>
	/// <param name="resultFlag">結果種別</param>
	/// <param name="resultData">結果情報</param>
	private sealed class ImportData<TParse, TStore>(char importCode, ParserData<TParse>? parserData, int invokeSize, bool resultFlag, TStore? resultData) {
		#region プロパティー定義
		/// <summary>
		/// 取込文字を取得します。
		/// </summary>
		/// <value>取込文字</value>
		public char ImportCode {
			get;
		} = importCode;
		/// <summary>
		/// 項目情報を取得します。
		/// </summary>
		/// <value>項目情報</value>
		public ParserData<TParse>? ParserData {
			get;
		} = parserData;
		/// <summary>
		/// 実行回数を取得します。
		/// </summary>
		/// <value>実行回数</value>
		public int InvokeSize {
			get;
		} = invokeSize;
		/// <summary>
		/// 結果種別を取得します。
		/// </summary>
		/// <value>結果種別</value>
		public bool ResultFlag {
			get;
		} = resultFlag;
		/// <summary>
		/// 結果情報を取得します。
		/// </summary>
		/// <value>結果情報</value>
		public TStore? ResultData {
			get;
		} = resultData;
		#endregion プロパティー定義

		#region 検証メソッド定義:InvokeTest
		/// <summary>
		/// 引数情報を検証します。
		/// </summary>
		/// <param name="sourceData">復号処理</param>
		/// <param name="parserCode">解析処理</param>
		public void InvokeTest(DsvDecodeData<TParse, TStore> sourceData, ParserCode<TParse> parserCode) {
			parserCode.ParserData = ParserData;
			var resultFlag = sourceData.ImportData(ImportCode, out var resultData);
			var invokeSize = parserCode.InvokeSize;
			Assert.Multiple(() => {
				Assert.That(invokeSize, Is.EqualTo(InvokeSize));
				Assert.That(resultFlag, Is.EqualTo(ResultFlag));
				Assert.That(resultData, Is.EqualTo(ResultData));
			});
		}
		#endregion 検証メソッド定義:InvokeTest
	}
	#endregion 非公開クラス定義:ImportData

	#region 非公開クラス定義:FinishData
	/// <summary>
	/// 終了情報クラスです。
	/// </summary>
	/// <typeparam name="TParse">項目種別</typeparam>
	/// <typeparam name="TStore">要素種別</typeparam>
	/// <param name="parserData">項目情報</param>
	/// <param name="invokeSize">実行回数</param>
	/// <param name="resultFlag">結果種別</param>
	/// <param name="resultData">結果情報</param>
	private sealed class FinishData<TParse, TStore>(ParserData<TParse>? parserData, int invokeSize, bool resultFlag, TStore? resultData) {
		#region プロパティー定義
		/// <summary>
		/// 項目情報を取得します。
		/// </summary>
		/// <value>項目情報</value>
		public ParserData<TParse>? ParserData {
			get;
		} = parserData;
		/// <summary>
		/// 実行回数を取得します。
		/// </summary>
		/// <value>実行回数</value>
		public int InvokeSize {
			get;
		} = invokeSize;
		/// <summary>
		/// 結果種別を取得します。
		/// </summary>
		/// <value>結果種別</value>
		public bool ResultFlag {
			get;
		} = resultFlag;
		/// <summary>
		/// 結果情報を取得します。
		/// </summary>
		/// <value>結果情報</value>
		public TStore? ResultData {
			get;
		} = resultData;
		#endregion プロパティー定義

		#region 検証メソッド定義:InvokeTest
		/// <summary>
		/// 引数情報を検証します。
		/// </summary>
		/// <param name="sourceData">復号処理</param>
		/// <param name="parserCode">解析処理</param>
		public void InvokeTest(DsvDecodeData<TParse, TStore> sourceData, ParserCode<TParse> parserCode) {
			parserCode.ParserData = ParserData;
			var resultFlag = sourceData.FinishData(out var resultData);
			var invokeSize = parserCode.InvokeSize;
			Assert.Multiple(() => {
				Assert.That(invokeSize, Is.EqualTo(InvokeSize));
				Assert.That(resultFlag, Is.EqualTo(ResultFlag));
				Assert.That(resultData, Is.EqualTo(ResultData));
			});
		}
		#endregion 検証メソッド定義:InvokeTest
	}
	#endregion 非公開クラス定義:FinishData
}
