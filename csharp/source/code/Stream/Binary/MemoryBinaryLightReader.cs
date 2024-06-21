namespace Occhitta.Libraries.Stream.Binary;

/// <summary>
/// 簡易バイナリ読込処理クラスです。
/// </summary>
/// <param name="source">要素配列</param>
/// <exception cref="ArgumentNullException"><paramref name="source" />に<c>Null</c>が指定された場合</exception>
public sealed class MemoryBinaryLightReader(byte[] source) : BinaryLightReader {
	#region メンバー変数定義
	/// <summary>
	/// 読込位置
	/// </summary>
	private int offset = 0;
	/// <summary>
	/// 要素配列
	/// </summary>
	private byte[]? source = source;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 要素配列を取得します。
	/// </summary>
	/// <value>要素配列</value>
	private byte[] Source => this.source ?? throw new ObjectDisposedException(GetType().FullName);
	#endregion プロパティー定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		this.offset = default;
		this.source = default;
		GC.SuppressFinalize(this);
	}
	#endregion 破棄メソッド定義

	#region 実装メソッド定義
	/// <summary>
	/// 後続情報を読込みます。
	/// <para>終端に達した場合、負数を返却します</para>
	/// </summary>
	/// <returns>後続情報</returns>
	public int Read() {
		var source = Source;
		if (this.offset < source.Length) {
			var result = source[this.offset];
			this.offset ++;
			return result;
		} else {
			return -1;
		}
	}
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	public int Read(byte[] buffer, int offset, int length) {
		for (var index = 0; index < length; index ++) {
			var choose = Read();
			if (choose == -1) {
				return index;
			} else {
				buffer[offset + index] = (byte)choose;
			}
		}
		return length;
	}
	#endregion 実装メソッド定義
}
