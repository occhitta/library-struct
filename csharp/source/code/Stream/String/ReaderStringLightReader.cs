namespace Occhitta.Libraries.Stream.String;

/// <summary>
/// <see cref="System.IO.TextReader" />利用読込処理クラスです。
/// </summary>
/// <param name="source">読込処理</param>
/// <exception cref="ArgumentNullException"><paramref name="source" />が<c>Null</c>である場合</exception>
public sealed class ReaderStringLightReader(System.IO.TextReader source) : StringLightReader {
	#region メンバー変数定義
	/// <summary>読込処理</summary>
	private System.IO.TextReader? source = source ?? throw new ArgumentNullException(nameof(source));
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 読込処理を取得します。
	/// </summary>
	/// <value>読込処理</value>
	/// <exception cref="ObjectDisposedException">当該情報が破棄された場合</exception>
	private System.IO.TextReader Source => this.source ?? throw new ObjectDisposedException(GetType().FullName);
	#endregion プロパティー定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	private void Dispose() {
		this.source = default;
	}
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	~ReaderStringLightReader() => Dispose();
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		Dispose();
		GC.SuppressFinalize(this);
	}
	#endregion 破棄メソッド定義

	#region 実装メソッド定義:StringHeavyReader
	/// <summary>
	/// 後続情報を読込みます。
	/// <para>終端に達した場合、負数を返却します</para>
	/// </summary>
	/// <returns>後続情報</returns>
	public int Read() => Source.Read();
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	public int Read(char[] buffer, int offset, int length) => Source.Read(buffer, offset, length);
	#endregion 実装メソッド定義:StringHeavyReader
}
