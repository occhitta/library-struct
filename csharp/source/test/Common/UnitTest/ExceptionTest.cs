using System.Collections;
using System.Reflection;

namespace Occhitta.Libraries.Common.UnitTest;

/// <summary>
/// <see cref="Exception" />検証クラスです。
/// </summary>
[Obsolete("Occhitta.Libraries.Testerライブラリに変更")]
public static class ExceptionTest {
	#region 内部メソッド定義:ToIndent
	/// <summary>
	/// 階層内容へ変換します。
	/// </summary>
	/// <param name="source">階層番号</param>
	/// <returns>階層内容</returns>
	private static string ToIndent(int source) =>
		new(' ', source * 2);
	#endregion 内部メソッド定義:ToIndent

	#region 内部メソッド定義:ToString:IDictionary関連
	/// <summary>
	/// 要素内容へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="indent">階層番号</param>
	/// <returns>要素内容</returns>
	private static string ToString(DictionaryEntry source, int indent) =>
		$"{ToString(source.Key, indent)} : {ToString(source.Value, indent)}";
	/// <summary>
	/// 要素内容へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="indent">階層番号</param>
	/// <returns>要素内容</returns>
	private static string ToString(IDictionaryEnumerator source, int indent) {
		var result = new StringBuilder();
		result.Append('{');
		while (source.MoveNext()) {
			var choose = source.Entry;
			result.Append(Environment.NewLine);
			result.Append(ToIndent(indent + 1));
			result.Append(ToString(choose, indent + 1));
		}
		result.Append(ToIndent(indent));
		result.Append('}');
		if (source is IDisposable finish) {
			finish.Dispose();
		}
		return result.ToString();
	}
	/// <summary>
	/// 辞書内容へ変換します。
	/// </summary>
	/// <param name="source">辞書情報</param>
	/// <param name="indent">階層番号</param>
	/// <returns>辞書内容</returns>
	private static string ToString(IDictionary source, int indent) =>
		ToString(source.GetEnumerator(), indent);
	/// <summary>
	/// 要素内容へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="indent">階層番号</param>
	/// <returns>要素内容</returns>
	private static string ToString(KeyValuePair<string, object?> source, int indent) =>
		$"{source.Key} : {ToString(source.Value, indent)}";
	/// <summary>
	/// 要素内容へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="indent">階層番号</param>
	/// <param name="prefix">先頭情報</param>
	/// <returns>要素内容</returns>
	private static string ToString(IReadOnlyDictionary<string, object?> source, int indent, Type prefix) {
		var result = new StringBuilder();
		result.Append($"{prefix} {{");
		foreach (var choose in source) {
			result.Append(Environment.NewLine);
			result.Append(ToIndent(indent + 1));
			result.Append(ToString(choose, indent + 1));
		}
		result.Append($"{Environment.NewLine}{ToIndent(indent)}}}");
		return result.ToString();
	}
	#endregion 内部メソッド定義:ToString:IDictionary関連

	#region 内部メソッド定義:ToString:Object関連
	/// <summary>
	/// 要素内容へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="indent">階層番号</param>
	/// <returns>要素内容</returns>
	private static string ToString(object? source, int indent) {
		if (source == null) {
			return "Null";
		} else if (source is OmitData) {
			return "...";
		} else if (source is bool) {
			return $"{source}";
		} else if (source is sbyte) {
			return $"{source}B";
		} else if (source is byte) {
			return $"{source}UB";
		} else if (source is short) {
			return $"{source}S";
		} else if (source is ushort) {
			return $"{source}US";
		} else if (source is int) {
			return $"{source}";
		} else if (source is uint) {
			return $"{source}U";
		} else if (source is long) {
			return $"{source}L";
		} else if (source is ulong) {
			return $"{source}UL";
		} else if (source is nint) {
			return $"{source}N";
		} else if (source is nuint) {
			return $"{source}NU";
		} else if (source is float) {
			return $"{source}F";
		} else if (source is double) {
			return $"{source}D";
		} else if (source is decimal) {
			return $"{source}M";
		} else if (source is System.Numerics.BigInteger) {
			return $"{source}BI";
		} else if (source is DateOnly) {
			return $"{source:yyyy-MM-dd}";
		} else if (source is TimeOnly) {
			return $"{source:HH:mm:ss.fffffff}";
		} else if (source is DateTime) {
			return $"{source:yyyy-MM-dd'T'HH:mm:ss.fffffff}";
		} else if (source is DateTime) {
			return $"{source:yyyy-MM-dd'T'HH:mm:ss.fffffff.zzz}";
		} else if (source is TimeSpan) {
			return $"{source:+dd\\.hh\\:mm\\:ss.fffffff}";
		} else if (source is MethodBase) {
			return $"{source}";
		} else if (source is char cacheA) {
			return cacheA switch {
				'\''       => "'\\''",
				'\t'       => "'\\t'",
				'\r'       => "'\\r'",
				'\n'       => "'\\n'",
				< '\u0020' => $"'\\u{(int)cacheA:X04}'",
				_          => $"'{cacheA}'"
			};
		} else if (source is string cacheB) {
			var result = cacheB;
			foreach (var (older, newer) in new[] {("\\", "\\\\"), ("\"", "\\\""), ("\r", "\\r"), ("\n", "\\n"), ("\t", "\\t")}) {
				result = result.Replace(older, newer);
			}
			for (var index = '\u0000'; index < '\u0020'; index ++) {
				result = result.Replace(index.ToString(), $"\\u{index:X04}");
			}
			return $"\"{result}\"";
		} else if (source is IDictionary cacheC) {
			return ToString(cacheC, indent);
		} else {
			return $"({source.GetType()}){source}";
		}
	}
	#endregion 内部メソッド定義:ToString:Object関連

	#region 内部メソッド定義:ToSource:Exception関連
	/// <summary>
	/// 例外情報を要素辞書へ変換します。
	/// </summary>
	/// <param name="source">例外情報</param>
	/// <returns>要素辞書</returns>
	private static Dictionary<string, object?> ToSource(Exception source) {
		var result = new Dictionary<string, object?>();
		var cache1 = source.GetType();
		var cache2 = cache1.GetProperties();
		foreach (var cache3 in cache2) {
			if (cache3.Name == "StackTrace") {
				result.Add("StackTrace", OmitData.Instance);
			} else {
				result.Add(cache3.Name, cache3.GetValue(source));
			}
		}
		return result;
	}
	#endregion 内部メソッド定義:ToSource:Exception関連

	#region 内部メソッド定義:ToString:Exception関連
	/// <summary>
	/// 例外内容へ変換します。
	/// </summary>
	/// <param name="source">例外情報</param>
	/// <param name="indent">階層番号</param>
	/// <returns>例外内容</returns>
	private static string ToString(Exception source, int indent) =>
		ToString(ToSource(source), indent, source.GetType());
	#endregion 内部メソッド定義:ToString:Exception関連

	#region 公開メソッド定義:ToString
	/// <summary>
	/// 例外内容へ変換します。
	/// </summary>
	/// <param name="source">例外情報</param>
	/// <returns>例外内容</returns>
	public static string ToString(Exception? source) {
		if (source == null) {
			return String.Empty;
		} else {
			return ToString(source, 0);
		}
	}
	#endregion 公開メソッド定義:ToString

	#region 非公開クラス定義
	/// <summary>
	/// 省略情報クラスです。
	/// </summary>
	private sealed class OmitData {
		#region メンバー変数定義
		/// <summary>
		/// インスタンス
		/// </summary>
		private static OmitData? instance;
		#endregion メンバー変数定義

		#region プロパティー定義
		/// <summary>
		/// インスタンスを取得します。
		/// </summary>
		/// <returns>インスタンス</returns>
		public static OmitData Instance => instance ??= new OmitData();
		#endregion プロパティー定義

		#region 生成メソッド定義
		/// <summary>
		/// 省略情報を生成します。
		/// </summary>
		private OmitData() {
			// 処理なし
		}
		#endregion 生成メソッド定義

		#region 継承メソッド定義
		/// <summary>
		/// 当該情報を表現文字列へ変換します。
		/// </summary>
		/// <returns>表現文字列</returns>
		public override string ToString() => "...";
		#endregion 継承メソッド定義
	}
	#endregion 非公開クラス定義
}
