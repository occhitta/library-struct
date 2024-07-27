namespace Occhitta.Libraries.Stream.String;

/// <summary>
/// <see cref="BinaryLightReader" />情報管理クラスです。
/// <para><see cref="BinaryLightReader" />の読込情報を管理する機能を提供します。</para>
/// </summary>
internal sealed class BinaryStringLightBuffer : IDisposable {
	#region メンバー変数定義
	/// <summary>読込処理</summary>
	private BinaryLightReader? source;
	/// <summary>読込配列</summary>
	private byte[]? buffer;
	/// <summary>読込個数</summary>
	private int length;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 読込処理を取得します。
	/// </summary>
	/// <value>読込処理</value>
	/// <exception cref="ObjectDisposedException">当該情報が破棄された場合</exception>
	private BinaryLightReader Source => this.source ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 読込配列を取得します。
	/// </summary>
	/// <value>読込配列</value>
	/// <exception cref="ObjectDisposedException">当該情報が破棄された場合</exception>
	public byte[] Buffer => this.buffer ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 読込個数を取得します。
	/// </summary>
	/// <value>読込個数</value>
	public int Length {
		get => this.length;
		private set => this.length = value;
	}
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 読込情報を生成します。
	/// </summary>
	/// <param name="source">読込処理</param>
	/// <param name="length">読込個数</param>
	/// <exception cref="ArgumentNullException"><paramref name="source" />が<c>Null</c>である場合</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="length" />が正数ではない場合</exception>
	public BinaryStringLightBuffer(BinaryLightReader source, int length) {
		if (source == null) {
			throw new ArgumentNullException(nameof(source));
		} else if (length <= 0) {
			throw new ArgumentOutOfRangeException(nameof(length), length, "Value must be positive.");
		} else {
			this.source = source;
			this.buffer = new byte[length];
			this.length = 0;
		}
	}
	#endregion 生成メソッド定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	private void Dispose() {
		this.source = default;
		this.buffer = default;
		this.length = default;
	}
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	~BinaryStringLightBuffer() => Dispose();
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		Dispose();
		GC.SuppressFinalize(this);
	}
	#endregion 破棄メソッド定義

	#region 公開メソッド定義:Import
	/// <summary>
	/// 読込処理を実行します。
	/// </summary>
	/// <returns>後続情報が存在した場合、<c>True</c>を返却</returns>
	public bool Import() {
		var buffer = Buffer;
		Length = Source.Read(buffer, 0, buffer.Length);
		return 0 < Length;
	}
	#endregion 公開メソッド定義:Import
}
