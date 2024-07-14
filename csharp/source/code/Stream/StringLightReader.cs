namespace Occhitta.Libraries.Stream;

/// <summary>
/// テキスト読込処理定義体です。
/// </summary>
public interface StringLightReader : IDisposable {
	/// <summary>
	/// 後続情報を読込みます。
	/// <para>終端に達した場合、負数を返却します</para>
	/// </summary>
	/// <returns>後続情報</returns>
	public int Read();
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	public int Read(char[] buffer, int offset, int length);
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <returns>保存した個数を返却</returns>
	public int Read(char[] buffer) => Read(buffer, 0, buffer.Length);
}
