using System.Threading;

namespace Occhitta.Libraries.Stream;

/// <summary>
/// テキスト読込処理定義体です。
/// </summary>
public interface StringHeavyReader : IDisposable {
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="cancel">取消処理</param>
	/// <returns>後続情報</returns>
	public Task<int> Read(CancellationToken cancel = default);
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <param name="cancel">取消処理</param>
	/// <returns>保存した個数を返却</returns>
	public Task<int> Read(char[] buffer, int offset, int length, CancellationToken cancel = default);
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="cancel">取消処理</param>
	/// <returns>保存した個数を返却</returns>
	public Task<int> Read(char[] buffer, CancellationToken cancel = default) => Read(buffer, 0, buffer.Length, cancel);
}
