using System.Globalization;
using Occhitta.Libraries.Common.UnitTest;

namespace Occhitta.Libraries.Common;

/// <summary>
/// <see cref="FixedBinaryData" />検証クラスです。
/// </summary>
public class FixedBinaryDataTest {
	#region 内部メソッド定義:ToData/ToText
	/// <summary>
	/// バイナリへ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <returns>バイナリ</returns>
	private static byte[] ToData(string source) {
		var choose = source.Replace("-", "").Replace(":", "");
		var result = new byte[choose.Length / 2];
		for (var index = 0; index < result.Length; index ++) {
			result[index] = Byte.Parse(choose.Substring(index, 2), NumberStyles.AllowHexSpecifier);
		}
		return result;
	}
	/// <summary>
	/// 例外内容へ変換します。
	/// </summary>
	/// <param name="param1">引数情報</param>
	/// <param name="param2">引数情報</param>
	/// <param name="param3">引数情報</param>
	/// <param name="param4">引数情報</param>
	/// <returns>例外内容</returns>
	private static string ToText(string param1, string param2, string param3, string param4) => $@"System.ArgumentOutOfRangeException {{
	Message : ""Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index'){param1}""
	ActualValue : {param2}
	ParamName : ""index""
	TargetSite : {param3}
	Data : {{
	}}
	InnerException : Null
	HelpLink : Null
	Source : ""{param4}""
	HResult : -2146233086
	StackTrace : ...
}}";
	/// <summary>
	/// 例外内容へ変換します。
	/// </summary>
	/// <param name="param1">引数情報</param>
	/// <param name="param2">引数情報</param>
	/// <param name="param3">引数情報</param>
	/// <returns>例外内容</returns>
	private static string ToText(int? param1, string param2, string param3) {
		var cache1 = param1 == null? "": (Environment.NewLine.Replace("\r", "\\r").Replace("\n", "\\n") + $"Actual value was {param1}.");
		var cache2 = param1 == null? "Null": $"{param1}";
		return ToText(cache1, cache2, param2, param3);
	}
	#endregion 内部メソッド定義:ToData/ToText

	#region 検証メソッド定義:SuccessCode
	/// <summary>
	/// 正常処理を検証します。
	/// </summary>
	/// <param name="source">要素配列</param>
	[TestCase("")]
	[TestCase("01")]
	[TestCase("010203")]
	public void SuccessCode(string source) {
		var choose = ToData(source);
		var actual = new FixedBinaryData(choose);
		Assert.That(actual, Has.Count.EqualTo(choose.Length));
		Assert.Multiple(() => {
			for (var index = 0; index < choose.Length; index ++) {
				Assert.That(choose[index], Is.EqualTo(actual[index]), "index={0}", index);
			}
		});
	}
	#endregion 検証メソッド定義:SuccessCode

	#region 検証メソッド定義:FailureCode
	/// <summary>
	/// 異常処理を検証します。
	/// </summary>
	/// <param name="source">要素配列</param>
	/// <param name="offset">取得番号</param>
	/// <param name="mscode">コア種別</param>
	private static void FailureCode(IReadOnlyList<byte> source, int offset, bool mscode) {
		static byte Invoke(IReadOnlyList<byte> source, int offset) => source[offset];
		var cache1 = mscode? (int?)null: offset;
		var cache2 = mscode? "(System.Reflection.RuntimeMethodInfo)Void ThrowArgumentOutOfRange_IndexMustBeLessException()": "(System.Reflection.RuntimeMethodInfo)Byte GetData(Int32)";
		var cache3 = mscode? "System.Private.CoreLib": "Occhitta.Libraries.Struct";
		var actual = Assert.Throws<ArgumentOutOfRangeException>(() => Invoke(source, offset));
		Assert.That(ExceptionUtilities.ToString(actual, "\t"), Is.EqualTo(ToText(cache1, cache2, cache3)));
	}
	/// <summary>
	/// 異常処理を検証します。
	/// </summary>
	[Test]
	public void Failure() {
		var value1 = new FixedBinaryData([1, 2]);
		var value2 = new List<byte>([1, 2]);
		Assert.Multiple(() => {
			FailureCode(value1, -1, false);
			FailureCode(value1,  2, false);
			FailureCode(value2, -1, true );
			FailureCode(value2,  2, true );
		});
	}
	#endregion 検証メソッド定義:FailureCode

	#region 検証メソッド定義:BinaryData
	/// <summary>
	/// <see cref="BinaryData" />を検証します。
	/// </summary>
	[TestFixture]
	public sealed class BinaryTest : IEnumerableTest<byte> {
		/// <summary>
		/// <see cref="DsvSourceData" />を検証します。
		/// </summary>
		/// <param name="source">引数情報</param>
		[TestCase("")]
		[TestCase("00")]
		[TestCase("00:01")]
		public void Test(string source) =>
			Test(new FixedBinaryData(ToData(source)), ToData(source));
	}
	#endregion 検証メソッド定義:BinaryData
}
