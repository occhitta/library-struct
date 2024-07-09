namespace Occhitta.Libraries.Struct.Dsv;

/// <summary>
/// DSV用結果情報クラスです。
/// </summary>
/// <typeparam name="TValue">項目種別</typeparam>
public sealed class DsvResultData<TValue> {
	#region メンバー変数定義
	/// <summary>結果種別</summary>
	private readonly bool state;
	/// <summary>項目情報</summary>
	private readonly ElementData<TValue> value;
	/// <summary>開始位置</summary>
	private readonly int index;
	/// <summary>要素個数</summary>
	private readonly int count;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 結果種別を取得します。
	/// </summary>
	/// <value>結果種別</value>
	public bool State => this.state;
	/// <summary>
	/// 項目情報を取得します。
	/// </summary>
	/// <value>項目情報</value>
	/// <exception cref="InvalidOperationException"><see cref="State" />が<c>False</c>の場合</exception>
	public TValue Value => this.value.Data;
	/// <summary>
	/// 開始位置を取得します。
	/// </summary>
	/// <value>開始位置</value>
	public int Index => this.index;
	/// <summary>
	/// 要素個数を取得します。
	/// </summary>
	/// <value>要素個数</value>
	public int Count => this.count;
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// DSV用結果情報を生成します。
	/// </summary>
	/// <param name="state">結果種別</param>
	/// <param name="value">項目情報</param>
	/// <param name="index">開始位置</param>
	/// <param name="count">要素個数</param>
	private DsvResultData(bool state, ElementData<TValue> value, int index, int count) {
		this.state = state;
		this.value = value;
		this.index = index;
		this.count = count;
	}
	/// <summary>
	/// DSV用成功情報を生成します。
	/// </summary>
	/// <param name="value">項目情報</param>
	/// <returns>成功情報</returns>
	public static DsvResultData<TValue> Success(TValue value) =>
		new(true, new SuccessData<TValue>(value), 0, 0);
	/// <summary>
	/// DSV用失敗情報を生成します。
	/// </summary>
	/// <param name="index">開始位置</param>
	/// <param name="count">要素個数</param>
	/// <returns>失敗情報</returns>
	public static DsvResultData<TValue> Failure(int index, int count) =>
		new(false, new FailureData<TValue>(), index, count);
	#endregion 生成メソッド定義

	#region 非公開クラス定義:ElementData/SuccessData/FailureData
	/// <summary>
	/// 要素情報定義体です。
	/// </summary>
	/// <typeparam name="TSource">項目種別</typeparam>
	private interface ElementData<TSource> {
		/// <summary>
		/// 要素情報を取得します。
		/// </summary>
		/// <value>要素情報</value>
		public TSource Data {
			get;
		}
	}
	/// <summary>
	/// 成功情報クラスです。
	/// </summary>
	/// <typeparam name="TSource">項目種別</typeparam>
	private sealed class SuccessData<TSource>(TSource data) : ElementData<TSource> {
		/// <summary>
		/// 要素情報を取得します。
		/// </summary>
		/// <value>要素情報</value>
		public TSource Data {
			get;
		} = data;
	}
	/// <summary>
	/// 失敗情報クラスです。
	/// </summary>
	/// <typeparam name="TSource">項目種別</typeparam>
	private sealed class FailureData<TSource> : ElementData<TSource> {
		/// <summary>
		/// 要素情報を取得します。
		/// </summary>
		/// <value>要素情報</value>
		public TSource Data => throw new InvalidOperationException("成功情報ではありません。");
	}
	#endregion 非公開クラス定義:ElementData/SuccessData/FailureData
}
