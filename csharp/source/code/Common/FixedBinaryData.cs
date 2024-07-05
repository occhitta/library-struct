namespace Occhitta.Libraries.Common;

/// <summary>
/// 不変バイナリ情報要素体です。
/// </summary>
/// <param name="source">要素集合</param>
public sealed class FixedBinaryData(IEnumerable<byte> source) : BinaryData {
	#region メンバー変数定義
	/// <summary>要素配列</summary>
	private readonly byte[] source = [.. source];
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 要素個数を取得します。
	/// </summary>
	/// <value>要素個数</value>
	public int Count => this.source.Length;
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="index">要素番号</param>
	/// <returns>要素情報</returns>
	public byte this[int index] => GetData(index);
	#endregion プロパティー定義

	#region 内部メソッド定義:GetData
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="index">要素番号</param>
	/// <returns>要素情報</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="index" />が負数である場合</exception>
	public byte GetData(int index) {
		if (index < 0 || this.source.Length <= index) {
			throw new ArgumentOutOfRangeException(nameof(index), index, "Index was out of range. Must be non-negative and less than the size of the collection.");
		} else {
			return this.source[index];
		}
	}
	#endregion 内部メソッド定義:GetData

	#region 実装メソッド定義:IEnumerable
	/// <summary>
	/// 反復処理を取得します。
	/// </summary>
	/// <returns>反復処理</returns>
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
		foreach (var choose in this.source) {
			yield return choose;
		}
	}
	/// <summary>
	/// 反復処理を取得します。
	/// </summary>
	/// <returns>反復処理</returns>
	IEnumerator<byte> IEnumerable<byte>.GetEnumerator() {
		foreach (var choose in this.source) {
			yield return choose;
		}
	}
	#endregion 実装メソッド定義

	#region 継承メソッド定義
	#endregion 継承メソッド定義
}
