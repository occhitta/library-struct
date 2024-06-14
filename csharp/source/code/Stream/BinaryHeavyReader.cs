namespace Occhitta.Libraries.Stream;

/// <summary>
/// バイナリ読込処理インターフェースです。
/// </summary>
#pragma warning disable IDE1006
public interface BinaryHeavyReader : IDisposable {
#pragma warning restore IDE1006
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	Task<int> Read();
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	Task<int> Read(byte[] buffer, int offset, int length);
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <returns>保存した個数を返却</returns>
	Task<int> Read(byte[] buffer) => Read(buffer, 0, buffer.Length);
}
