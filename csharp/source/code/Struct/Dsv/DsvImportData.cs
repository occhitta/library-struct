namespace Occhitta.Libraries.Struct.Dsv;

/// <summary>
/// DSV形式取込情報クラスです。
/// <para>当該情報は不変情報となります。</para>
/// </summary>
public sealed class DsvImportData : DsvSourceData<string>, IEquatable<DsvImportData> {
	#region メンバー変数定義
	/// <summary>
	/// 項目配列
	/// </summary>
	private readonly string[] source;
	/// <summary>
	/// 検索番号
	/// </summary>
	private int? number;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 項目個数を取得します。
	/// </summary>
	/// <value>項目個数</value>
	public int Count => this.source.Length;
	/// <summary>
	/// 項目内容を取得します。
	/// </summary>
	/// <param name="index">項目番号</param>
	/// <returns>項目内容</returns>
	public string this[int index] => this.source[index];
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// DSV形式取込情報を生成します。
	/// </summary>
	/// <param name="source">項目集合</param>
	/// <exception cref="ArgumentNullException"><paramref name="source" />自体が<c>Null</c>となっている場合</exception>
	/// <exception cref="ArgumentNullException"><paramref name="source" />要素が<c>Null</c>となっている場合</exception>
	internal DsvImportData(IEnumerable<string> source) {
		if (source == null) {
			throw new ArgumentNullException(nameof(source));
		} else {
			this.source = [..source];
			this.number = default;
			for (var index = 0; index < this.source.Length; index ++) {
				if (this.source[index] == null) throw new ArgumentNullException($"{nameof(source)}[{index}]");
			}
		}
	}
	/// <summary>
	/// DSV形式取込情報を生成します。
	/// </summary>
	/// <param name="source">項目集合</param>
	/// <returns>DSV形式取込情報</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" />自体が<c>Null</c>となっている場合</exception>
	/// <exception cref="ArgumentNullException"><paramref name="source" />要素が<c>Null</c>となっている場合</exception>
	private static DsvImportData CreateData(IEnumerable<string> source) =>
		new(source);
	/// <summary>
	/// DSV形式取込情報を生成します。
	/// </summary>
	/// <param name="reader">読込処理</param>
	/// <param name="escape">制御文字</param>
	/// <param name="marker">区切文字</param>
	/// <param name="parser">解析処理</param>
	/// <returns>DSV形式取込集合</returns>
	/// <exception cref="StructException">復号処理に失敗した場合</exception>
	public static IEnumerable<DsvImportData> DecodeData(StringLightReader reader, char escape, char marker, DsvParserCode<string> parser) {
		using var source = new DsvDecodeData<string, DsvImportData>(parser, CreateData, escape, marker);
		while (true) {
			var choose = reader.Read();
			if (choose < 0) {
				break;
			} else if (source.ImportData((char)choose, out var record)) {
				yield return record;
			}
		}
		if (source.FinishData(out var finish)) {
			yield return finish;
		}
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義:IsEquals
	/// <summary>
	/// 引数情報が等価であるか判定します。
	/// </summary>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <returns>等価である場合、<c>True</c>を返却</returns>
	private static bool IsEquals(string[] value1, string[] value2) {
		if (value1.Length == value2.Length) {
			// 要素個数が同数である場合
			for (var index = 0; index < value1.Length; index ++) {
				if (!String.Equals(value1[index], value2[index])) return false;
			}
			return true;
		} else {
			// 要素個数が異数である場合
			return false;
		}
	}
	#endregion 内部メソッド定義:IsEquals

	#region 内部メソッド定義:ToNumber
	/// <summary>
	/// 引数情報のハッシュ値を算出します。
	/// </summary>
	/// <param name="source">引数情報</param>
	/// <returns>ハッシュ値</returns>
	private static int ToNumber(string source) {
		var result = 0;
		if (source != null) {
			foreach (var choose in source) {
				result = Tuple.Create(result, choose).GetHashCode();
			}
		}
		return result;
	}
	/// <summary>
	/// 引数情報のハッシュ値を算出します。
	/// </summary>
	/// <param name="values">引数情報</param>
	/// <returns>ハッシュ値</returns>
	private static int ToNumber(string[] values) {
		var result = 0;
		for (var index = 0; index < values.Length; index ++) {
			result = Tuple.Create(result, index, ToNumber(values[index])).GetHashCode();
		}
		return result;
	}
	#endregion 内部メソッド定義:ToNumber

	#region 内部メソッド定義:ToString
	/// <summary>
	/// 引数配列を表現文字列へ変換します。
	/// </summary>
	/// <param name="source">引数配列</param>
	/// <param name="escape">制御文字</param>
	/// <param name="anchor">区切文字</param>
	/// <returns>表現文字列</returns>
	private static string ToString(string[] source, char escape, char anchor) {
		var result = new string[source.Length];
		for (var index = 0; index < result.Length; index ++) {
			var choose = source[index];
			var change = choose.Replace($"{escape}", $"{escape}{escape}");
			result[index] = $"{escape}{change}{escape}";
		}
		return String.Join(anchor, result);
	}
	#endregion 内部メソッド定義:ToString

	#region 実装メソッド定義:IReadOnlyList
	/// <summary>
	/// 反復処理を取得します。
	/// </summary>
	/// <returns>反復処理</returns>
	IEnumerator<string> IEnumerable<string>.GetEnumerator() {
		foreach (var choose in this.source) {
			yield return choose;
		}
	}
	#endregion 実装メソッド定義:IReadOnlyList

	#region 実装メソッド定義:IEquatable
	/// <summary>
	/// 当該情報と等価であるか判定します。
	/// </summary>
	/// <param name="some">判定情報</param>
	/// <returns>等価である場合、<c>True</c>を返却</returns>
	public bool Equals(DsvImportData? some) =>
		some != null && IsEquals(this.source, some.source);
	#endregion 実装メソッド定義:IEquatable

	#region 継承メソッド定義
	/// <summary>
	/// 当該情報と等価であるか判定します。
	/// </summary>
	/// <param name="some">判定情報</param>
	/// <returns>等価である場合、<c>True</c>を返却</returns>
	public override bool Equals(object? some) =>
		Equals(some as DsvImportData);
	/// <summary>
	/// 当該情報のハッシュ値を算出します。
	/// </summary>
	/// <returns>ハッシュ値</returns>
	public override int GetHashCode() =>
		this.number ??= ToNumber(this.source);
	/// <summary>
	/// 当該情報を表現文字列へ変換します。
	/// </summary>
	/// <returns>表現文字列</returns>
	public override string ToString() =>
		'[' + ToString(this.source, '"', ',') + ']';
	#endregion 継承メソッド定義
}
