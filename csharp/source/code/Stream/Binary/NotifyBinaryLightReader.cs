namespace Occhitta.Libraries.Stream.Binary;

/// <summary>
/// 通知バイナリ読込処理クラスです。
/// </summary>
/// <param name="source">読込処理</param>
/// <exception cref="ArgumentNullException"><paramref name="source" />が<c>Null</c>である場合</exception>
public sealed class NotifyBinaryLightReader(BinaryLightReader source) : BinaryLightReader {
	#region メンバー変数定義
	/// <summary>
	/// 読込処理
	/// </summary>
	private BinaryLightReader? source = source ?? throw new ArgumentNullException(nameof(source));
	/// <summary>
	/// 監視管理
	/// </summary>
	private EventHandler<int>? listen = null;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 読込処理を取得します。
	/// </summary>
	/// <value>読込処理</value>
	/// <exception cref="ObjectDisposedException">当該情報が破棄されている場合</exception>
	private BinaryLightReader Source => this.source ?? throw new ObjectDisposedException(GetType().FullName);
	#endregion プロパティー定義

	#region 公開イベント定義
	/// <summary>
	/// 受信処理を追加または削除します。
	/// </summary>
	public event EventHandler<int>? Listen {
		add    => this.listen += value;
		remove => this.listen -= value;
	}
	/// <summary>
	/// 読込個数を通知します。
	/// </summary>
	/// <param name="length">読込個数</param>
	private void Notify(int length) =>
		this.listen?.Invoke(this, length);
	#endregion 公開イベント定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		this.source = null;
		this.listen = null;
	}
	#endregion 破棄メソッド定義

	#region 実装メソッド定義
	/// <summary>
	/// 後続情報を読込みます。
	/// <para>終端に達した場合、負数を返却します</para>
	/// </summary>
	/// <returns>後続情報</returns>
	public int Read() {
		var result = Source.Read();
		if (0 <= result) Notify(1);
		return result;
	}
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	public int Read(byte[] buffer, int offset, int length) {
		var result = Source.Read(buffer, offset, length);
		if (0 <= result) Notify(result);
		return result;
	}
	#endregion 実装メソッド定義
}
