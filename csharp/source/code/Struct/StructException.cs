namespace Occhitta.Libraries.Struct;

/// <summary>
/// 構造例外クラスです。
/// </summary>
public class StructException : SystemException {
	#region プロパティー定義
	/// <summary>
	/// 基本内容を取得します。
	/// </summary>
	/// <value>基本内容</value>
	public string Reason {
		get;
	}
	/// <summary>
	/// 部分情報を取得します。
	/// </summary>
	/// <value>部分情報</value>
	public object Region {
		get;
	}
	/// <summary>
	/// 起点番号を取得します。
	/// </summary>
	/// <value>起点番号</value>
	public int Offset {
		get;
	}
	/// <summary>
	/// 要素個数を取得します。
	/// </summary>
	/// <value>要素個数</value>
	public int Length {
		get;
	}
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 構造例外を生成します。
	/// </summary>
	/// <param name="reason">基本内容</param>
	/// <param name="region">部分情報</param>
	/// <param name="offset">起点番号</param>
	/// <param name="length">要素個数</param>
	public StructException(string reason, string region, int offset , int length) : base(ToText(reason, region, offset, length)) {
		Reason = reason;
		Region = region;
		Offset = offset;
		Length = length;
	}
	/// <summary>
	/// 構造例外を生成します。
	/// </summary>
	/// <param name="reason">基本内容</param>
	/// <param name="region">部分情報</param>
	/// <param name="offset">起点番号</param>
	/// <param name="length">要素個数</param>
	public StructException(string reason, ReadOnlySpan<char> region, int offset, int length) : this(reason, region.ToString(), offset, length) {
		// 処理なし
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// 文言内容へ変換します。
	/// </summary>
	/// <param name="reason">基本内容</param>
	/// <param name="region">部分情報</param>
	/// <param name="offset">起点番号</param>
	/// <param name="length">要素個数</param>
	/// <returns>文言内容</returns>
	private static string ToText(string reason, string region, int offset, int length) {
		var value1 = new String(' ', offset);
		var value2 = new String('^', length);
		return $"{reason}{Environment.NewLine}Offset : {offset}{Environment.NewLine}Length : {length}{Environment.NewLine}Region : {region}{Environment.NewLine}       : {value1}{value2}";
	}
	#endregion 内部メソッド定義
}
