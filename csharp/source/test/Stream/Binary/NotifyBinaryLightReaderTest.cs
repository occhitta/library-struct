namespace Occhitta.Libraries.Stream.Binary;

/// <summary>
/// <see cref="NotifyBinaryLightReader" />検証クラスです。
/// </summary>
/// <remarks>
/// 1.<see cref="MemoryBinaryLightReader" />を利用する為、左記クラスが正しく動作する前提で検証します。
/// </remarks>
[TestFixture]
public class NotifyBinaryLightReaderTest {
	#region メンバー定数定義
	/// <summary>
	/// 実行名称:ライブラリ名称
	/// </summary>
	private const string GroupName = "Occhitta.Libraries.Struct";
	/// <summary>
	/// 経路名称:名前空間名称
	/// </summary>
	private const string RouteName = "Occhitta.Libraries.Stream.Binary";
	/// <summary>
	/// 種別名称:クラス名称
	/// </summary>
	private const string ClassName = $"{RouteName}.{nameof(NotifyBinaryLightReader)}";
	#endregion メンバー定数定義

	#region 内部メソッド定義:ToBinary
	/// <summary>
	/// 要素内容を生成します。
	/// </summary>
	/// <param name="source">乱数情報</param>
	/// <param name="length">要素個数</param>
	/// <returns>要素内容</returns>
	private static byte[] ToBinary(Random source, int length) {
		var result = new byte[length];
		for (var index = 0; index < length; index ++) {
			result[index] = (byte)source.Next();
		}
		return result;
	}
	#endregion 内部メソッド定義:ToBinary

	#region 検証メソッド定義:TestData
	/// <summary>
	/// <see cref="NotifyBinaryLightReader.Read()" />を検証します。
	/// </summary>
	/// <param name="sourceSize">要素個数</param>
	[TestCase(null, 000)]
	[TestCase(null, 001)]
	[TestCase(null, 255)]
	[TestCase(null, 256)]
	public void TestData(int? sourceCode, int sourceSize) {
		var randomCode = sourceCode ?? (new Random().Next());
		var sourceList = ToBinary(new Random(randomCode), sourceSize);
		using var parentData = new MemoryBinaryLightReader(sourceList);
		using var sourceData = new NotifyBinaryLightReader(parentData);
		sourceData.Listen += (source, length) => Assert.That(length, Is.EqualTo(1));
		for (var index = 0; ; index ++) {
			var result = sourceData.Read();
			if (result < 0) {
				break;
			} else {
				Assert.That(result, Is.EqualTo(sourceList[index]), "要素内容が異なります。(乱数番号：{0}, 要素個数：{1})", randomCode, sourceSize);
			}
		}
	}
	#endregion 検証メソッド定義:TestData

	#region 検証メソッド定義:TestList
	/// <summary>
	/// <see cref="NotifyBinaryLightReader.Read(byte[], int, int)" />を検証します。
	/// </summary>
	/// <param name="sourceCode">確認番号</param>
	/// <param name="sourceSize">要素個数</param>
	private static void TestList(int sourceCode, int sourceSize) {
		var randomCode = new Random(sourceCode);
		var sourceList = ToBinary(randomCode, sourceSize);
		var resultList = new List<byte>();
		var expectSize = 0;
		var remainSize = sourceList.Length;
		using var parentData = new MemoryBinaryLightReader(sourceList);
		using var sourceData = new NotifyBinaryLightReader(parentData);
		sourceData.Listen += (source, length) => expectSize = length;
		while (true) {
			var bufferSize = randomCode.Next(1, 1024);
			var bufferData = new byte[bufferSize];
			var resultSize = sourceData.Read(bufferData, 0, bufferData.Length);
			if (resultSize <= 0) {
				break;
			} else {
				Assert.That(expectSize, Is.EqualTo(Math.Min(remainSize, bufferData.Length)), "読込個数が異なります。(乱数番号：{0}, 要素個数：{1})", sourceCode, sourceSize);
				remainSize -= resultSize;
				for (var index = 0; index < resultSize; index ++) {
					resultList.Add(bufferData[index]);
				}
			}
		}
		Assert.That(resultList, Has.Count.EqualTo(sourceList.Length), "結果個数が異なります。(乱数番号：{0}, 要素個数：{1})", sourceCode, sourceSize);
		for (var index = 0; index < sourceList.Length; index ++) {
			Assert.That(resultList[index], Is.EqualTo(sourceList[index]), "結果内容が異なります。(乱数番号：{0}, 要素個数：{1})", sourceCode, sourceSize);
		}
	}
	/// <summary>
	/// <see cref="NotifyBinaryLightReader.Read(byte[], int, int)" />を検証します。
	/// </summary>
	/// <param name="sourceSize">要素個数</param>
	[TestCase(null, 000)]
	[TestCase(null, 001)]
	[TestCase(null, 255)]
	[TestCase(null, 256)]
	public void TestList(int? sourceCode, int sourceSize) {
		if (sourceCode == null) {
			var randomCode = new Random();
			for (var index = 0; index < 10; index ++) {
				TestList(randomCode.Next(), sourceSize);
			}
		} else {
			TestList(sourceCode.Value, sourceSize);
		}
	}
	#endregion 検証メソッド定義:TestList

	#region 検証メソッド定義:Disposed
	/// <summary>
	/// <see cref="IDisposable" />を検証します。
	/// </summary>
	[Test]
	public void Disposed() {
		const string     Phase1 = "";
		const string?    Phase2 = null;
		const int        Phase3 = -2146232798;
		const Exception? Phase4 = null;
		const string     Phase5 = $"Cannot access a disposed object.\r\nObject name: '{ClassName}'.";
		const string     Phase6 = GroupName;
		const string     Phase7 = "Occhitta.Libraries.Stream.BinaryLightReader get_Source()";
		const string     Phase8 = ClassName;
		using var parent = new MemoryBinaryLightReader([]);
		using var source = new NotifyBinaryLightReader(parent);
		((IDisposable)source).Dispose();
		var actual = Assert.Throws<ObjectDisposedException>(() => source.Read());
		Assert.Multiple(() => {
			var value1 = actual.Data;
			var value2 = actual.HelpLink;
			var value3 = actual.HResult;
			var value4 = actual.InnerException;
			var value5 = actual.Message;
			var value6 = actual.Source;
			var value7 = actual.TargetSite?.ToString();
			var value8 = actual.ObjectName;
			Assert.That(value1, Is.EqualTo(Phase1), "Failure {0}.", nameof(actual.Data));
			Assert.That(value2, Is.EqualTo(Phase2), "Failure {0}.", nameof(actual.HelpLink));
			Assert.That(value3, Is.EqualTo(Phase3), "Failure {0}.", nameof(actual.HResult));
			Assert.That(value4, Is.EqualTo(Phase4), "Failure {0}.", nameof(actual.InnerException));
			Assert.That(value5, Is.EqualTo(Phase5), "Failure {0}.", nameof(actual.Message));
			Assert.That(value6, Is.EqualTo(Phase6), "Failure {0}.", nameof(actual.Source));
			Assert.That(value7, Is.EqualTo(Phase7), "Failure {0}.", nameof(actual.TargetSite));
			Assert.That(value8, Is.EqualTo(Phase8), "Failure {0}.", nameof(actual.ObjectName));
		});
	}
	#endregion 検証メソッド定義:Disposed

	#region 検証メソッド定義:Coverage:コンストラクタ
	/// <summary>
	/// <see cref="NotifyBinaryLightReader(BinaryLightReader)" />を検証します。
	/// </summary>
	/// <param name="source">読込処理</param>
	[TestCase(null)]
	public void Coverage(BinaryLightReader source) {
		const string     Phase1 = "";
		const string?    Phase2 = null;
		const int        Phase3 = -2147467261;
		const Exception? Phase4 = null;
		const string     Phase5 = "Value cannot be null. (Parameter 'source')";
		const string     Phase6 = GroupName;
		const string     Phase7 = $"Void .ctor(Occhitta.Libraries.Stream.BinaryLightReader)";
		const string     Phase8 = "source";
		var actual = Assert.Throws<ArgumentNullException>(() => new NotifyBinaryLightReader(source));
		Assert.Multiple(() => {
			var value1 = actual.Data;
			var value2 = actual.HelpLink;
			var value3 = actual.HResult;
			var value4 = actual.InnerException;
			var value5 = actual.Message;
			var value6 = actual.Source;
			var value7 = actual.TargetSite?.ToString();
			var value8 = actual.ParamName;
			Assert.That(value1, Is.EqualTo(Phase1), "Failure {0}.", nameof(actual.Data));
			Assert.That(value2, Is.EqualTo(Phase2), "Failure {0}.", nameof(actual.HelpLink));
			Assert.That(value3, Is.EqualTo(Phase3), "Failure {0}.", nameof(actual.HResult));
			Assert.That(value4, Is.EqualTo(Phase4), "Failure {0}.", nameof(actual.InnerException));
			Assert.That(value5, Is.EqualTo(Phase5), "Failure {0}.", nameof(actual.Message));
			Assert.That(value6, Is.EqualTo(Phase6), "Failure {0}.", nameof(actual.Source));
			Assert.That(value7, Is.EqualTo(Phase7), "Failure {0}.", nameof(actual.TargetSite));
			Assert.That(value8, Is.EqualTo(Phase8), "Failure {0}.", nameof(actual.ParamName));
		});
	}
	#endregion 検証メソッド定義:Coverage:コンストラクタ

	#region 検証メソッド定義:Coverage:通知イベント
	/// <summary>
	/// <see cref="NotifyBinaryLightReader.Listen" />を検証します。
	/// </summary>
	[Test]
	public void Coverage() {
		var action = new EventHandler<int>((source, length) => Assert.Fail());
		using var parent = new MemoryBinaryLightReader([0]);
		using var source = new NotifyBinaryLightReader(parent);
		source.Listen += action;
		source.Listen -= action;
		source.Read();
	}
	#endregion 検証メソッド定義:Coverage:通知イベント
}
