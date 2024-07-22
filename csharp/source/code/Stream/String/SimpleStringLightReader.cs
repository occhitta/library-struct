namespace Occhitta.Libraries.Stream.String;

/// <summary>
/// 簡易読込処理クラスです。
/// </summary>
public sealed class SimpleStringLightReader : StringLightReader {
	#region メンバー変数定義
	/// <summary>読込情報</summary>
	private string? source;
	/// <summary>読込位置</summary>
	private int offset;
	/// <summary>要素個数</summary>
	private int length;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <value>要素情報</value>
	/// <exception cref="ObjectDisposedException">当該情報が破棄された場合</exception>
	private string Source => this.source ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 読込位置を取得します。
	/// </summary>
	/// <value>読込位置</value>
	private int Offset {
		get => this.offset;
		set => this.offset = value;
	}
	/// <summary>
	/// 要素個数を取得します。
	/// </summary>
	/// <value>要素個数</value>
	private int Length {
		get => this.length;
	}
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 簡易読込処理を生成します。
	/// </summary>
	/// <param name="source">読込情報</param>
	/// <exception cref="ArgumentNullException"><paramref name="source" />が<c>Null</c>である場合</exception>
	public SimpleStringLightReader(string source) {
		if (source == null) {
			throw new ArgumentNullException(nameof(source));
		} else {
			this.source = source;
			this.offset = 0;
			this.length = source.Length;
		}
	}
	#endregion 生成メソッド定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	private void Dispose() {
		this.source = default;
		this.offset = default;
		this.length = default;
	}
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	~SimpleStringLightReader() => Dispose();
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		Dispose();
		GC.SuppressFinalize(this);
	}
	#endregion 破棄メソッド定義

	#region 内部メソッド定義:Read
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="source">読込情報</param>
	/// <param name="offset">読込位置</param>
	/// <param name="length">要素個数</param>
	/// <param name="result">後続情報</param>
	/// <returns>後続情報が存在する場合、<c>True</c>を返却</returns>
	private static bool Read(string source, int offset, int length, out char result) {
		if (offset < length) {
			result = source[offset];
			return true;
		} else {
			result = default;
			return false;
		}
	}
	#endregion 内部メソッド定義:Read

	#region 実装メソッド定義:StringHeavyReader
	/// <summary>
	/// 後続情報を読込みます。
	/// <para>終端に達した場合、負数を返却します</para>
	/// </summary>
	/// <returns>後続情報</returns>
	public int Read() {
		if (Read(Source, Offset, Length, out var result)) {
			Offset ++;
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
	public int Read(char[] buffer, int offset, int length) {
		if (buffer == null) {
			throw new ArgumentNullException(nameof(buffer));
		} else if (offset < 0) {
			throw new ArgumentOutOfRangeException(nameof(offset), offset, $"{nameof(offset)} ('{offset}') must be a non-negative value.");
		} else if (length < 0) {
			throw new ArgumentOutOfRangeException(nameof(length), length, $"{nameof(length)} ('{length}') must be a non-negative value.");
		} else if (buffer.Length < offset + length) {
			throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
		} else {
			for (var index = 0; index < length; index ++) {
				var result = Read();
				if (result == -1) {
					return index;
				} else {
					buffer[index] = (char)result;
				}
			}
			return length;
		}
	}
	#endregion 実装メソッド定義:StringHeavyReader
}
