namespace Occhitta.Libraries.Stream.Binary;

/// <summary>
/// <see cref="Stream"/>利用バイナリ読込処理クラスです。
/// </summary>
/// <param name="source">読込処理</param>
/// <exception cref="ArgumentNullException"><paramref name="source" />に<c>Null</c>が指定された場合</exception>
public sealed class StreamBinaryLightReader(System.IO.Stream source) : BinaryLightReader {
	#region メンバー変数定義
	/// <summary>
	/// 読込処理
	/// </summary>
	private System.IO.Stream? stream = source ?? throw new ArgumentNullException(nameof(source));
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 読込処理を取得します。
	/// </summary>
	/// <returns>読込処理</returns>
	private System.IO.Stream Stream => this.stream ?? throw new ObjectDisposedException(GetType().FullName);
	#endregion プロパティー定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void System.IDisposable.Dispose() {
		this.stream = null;
		GC.SuppressFinalize(this);
	}
	#endregion 破棄メソッド定義

	#region 実装メソッド定義
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	public int Read() => Stream.ReadByte();
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	public int Read(byte[] buffer, int offset, int length) => Stream.Read(buffer, offset, length);
	#endregion 実装メソッド定義
}
