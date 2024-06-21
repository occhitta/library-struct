namespace Occhitta.Libraries.Stream;

/// <summary>
/// <see cref="BinaryLightReader" />検証クラスです。
/// </summary>
[TestFixture]
public class BinaryLightReaderTest : BinaryLightReader {
	#region 非公開ソース定義
	#region メンバー変数定義
	/// <summary>
	/// 読込位置
	/// </summary>
	private int offset;
	/// <summary>
	/// 要素配列
	/// </summary>
	private char[]? source;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 要素配列を取得します。
	/// </summary>
	/// <value>要素配列</value>
	private char[] Source => this.source ?? throw new ObjectDisposedException(GetType().FullName);
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

	#region 内部メソッド定義:ToSource
	/// <summary>
	/// 要素配列へ変換します。
	/// </summary>
	/// <param name="start">開始番号</param>
	/// <param name="count">要素個数</param>
	/// <returns>要素配列</returns>
	private static char[] ToSource(char start, int count) {
		var result = new char[count];
		for (var index = 0; index < count; index ++) {
			result[index] = start ++;
		}
		return result;
	}
	#endregion 内部メソッド定義:ToSource

	#region 内部メソッド定義:ToString
	/// <summary>
	/// 要素内容へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">要素個数</param>
	/// <returns>要素内容</returns>
	private static string ToString(char[] source, int offset, int length) =>
		new(source, offset, length);
	#endregion 内部メソッド定義:ToString

	#region 内部メソッド定義:ReadData
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	private async Task<int> ReadData() {
		var source = Source;
		var result = this.offset < source.Length? source[this.offset ++]: -1;
		return await Task.Run(() => result);
	}
	#endregion 内部メソッド定義:ReadData

	#region 実装メソッド定義:BinaryHeavyReader
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	int BinaryLightReader.Read() =>
		ReadData();
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <param name="cancel">取消処理</param>
	/// <returns>保存した個数を返却</returns>
	async Task<int> StringHeavyReader.Read(byte[] buffer, int offset, int length, CancellationToken cancel) {
		for (var index = 0; index < length; index ++) {
			var choose = await ReadData(cancel);
			if (choose < 0) {
				return index;
			} else {
				buffer[offset + index] = (byte)choose;
			}
		}
		return length;
	}
	#endregion 実装メソッド定義:BinaryHeavyReader
	#endregion 非公開ソース定義

	#region 実装メソッド定義
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	int BinaryLightReader.Read() => this.source < 256? this.source ++: -1;
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	int BinaryLightReader.Read(byte[] buffer, int offset, int length) {
		for (var index = 0; index < length; index ++) {
			var choose = this.source < 256? this.source ++: -1;
			if (choose == -1) {
				return index;
			} else {
				buffer[offset + index] = (byte)choose;
			}
		}
		return length;
	}
	#endregion 実装メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// <see cref="BinaryLightReader.Read(byte[])" />を検証します。
	/// </summary>
	/// <param name="source">検証情報</param>
	private static void Test(BinaryLightReader source) {
		var buffer = new byte[1024];
		Assert.That(source.Read(buffer), Is.EqualTo(256));
		for (var index = 0; index < buffer.Length; index ++) {
			Assert.That(buffer[index], Is.EqualTo(index < 256? index: 0));
		}
	}
	#endregion 内部メソッド定義

	#region 検証メソッド定義
	/// <summary>
	/// <see cref="BinaryLightReader.Read(byte[])" />を検証します。
	/// </summary>
	[Test]
	public void Test() => Test(this);
	#endregion 検証メソッド定義
}
