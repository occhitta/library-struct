namespace Occhitta.Libraries;

/// <summary>
/// 検証共通関数クラスです。
/// </summary>
public static class CommonCode {
	/// <summary>
	/// バイナリ情報へ変換します。
	/// </summary>
	/// <param name="source">処理情報</param>
	/// <returns>バイナリ情報</returns>
	public static byte[] ToData(ReadOnlySpan<char> source) {
		var result = new byte[source.Length / 2];
		for (var index = 0; index < result.Length; index ++) {
			result[index] = Byte.Parse(source[(index * 2)..(index * 2 + 2)], System.Globalization.NumberStyles.HexNumber);
		}
		return result;
	}
	/// <summary>
	/// テキスト情報へ変換します。
	/// </summary>
	/// <param name="source">処理情報</param>
	/// <returns>テキスト情報</returns>
	public static string ToText(byte source) =>
		$"{source:X2}";
	/// <summary>
	/// テキスト情報へ変換します。
	/// </summary>
	/// <param name="source">処理情報</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">要素個数</param>
	/// <returns>テキスト情報</returns>
	public static string ToText(ReadOnlySpan<byte> source, int offset, int length) {
		var result = new StringBuilder(length * 2);
		for (var index = 0; index < length; index ++) {
			result.AppendFormat("{0:X2}", source[offset + index]);
		}
		return result.ToString();
	}
}
