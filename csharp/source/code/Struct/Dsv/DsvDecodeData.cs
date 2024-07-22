namespace Occhitta.Libraries.Struct.Dsv;

/// <summary>
/// DSV形式復号処理クラスです。
/// </summary>
/// <typeparam name="TParse">項目種別</typeparam>
/// <typeparam name="TStore">要素種別</typeparam>
/// <param name="parserHook">解析処理</param>
/// <param name="exportHook">出力処理</param>
/// <param name="escapeCode">制御文字</param>
/// <param name="markerCode">区切文字</param>
internal sealed class DsvDecodeData<TParse, TStore>(DsvParserCode<TParse> parserHook, DsvResultCode<TParse, TStore> exportHook, char escapeCode, char markerCode) : IDisposable {
	#region メンバー変数定義
	/// <summary>解析処理</summary>
	private DsvParserCode<TParse>? parserHook = parserHook ?? throw new ArgumentNullException(nameof(parserHook));
	/// <summary>出力処理</summary>
	private DsvResultCode<TParse, TStore>? exportHook = exportHook ?? throw new ArgumentNullException(nameof(exportHook));
	/// <summary>制御文字</summary>
	private char escapeCode = escapeCode;
	/// <summary>区切文字</summary>
	private char markerCode = markerCode;
	/// <summary>保留情報(項目情報)</summary>
	private System.Text.StringBuilder? bufferData = new();
	/// <summary>項目一覧</summary>
	private List<TParse>? exportList = [];
	/// <summary>要素番号</summary>
	private int recordCode = 0;
	/// <summary>状態種別</summary>
	private bool ignoreFlag = false;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 解析処理を取得します。
	/// </summary>
	/// <value>解析処理</value>
	/// <exception cref="ObjectDisposedException">当該情報が破棄された場合</exception>
	private DsvParserCode<TParse> ParserHook => this.parserHook ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 出力処理を取得します。
	/// </summary>
	/// <value>出力処理</value>
	/// <exception cref="ObjectDisposedException">当該情報が破棄された場合</exception>
	private DsvResultCode<TParse, TStore> ExportHook => this.exportHook ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 制御文字を取得します。
	/// </summary>
	/// <value>制御文字</value>
	private char EscapeCode => this.escapeCode;
	/// <summary>
	/// 区切文字を取得します。
	/// </summary>
	/// <value>区切文字</value>
	private char MarkerCode => this.markerCode;
	/// <summary>
	/// 保留情報を取得します。
	/// </summary>
	/// <value>保留情報</value>
	/// <exception cref="ObjectDisposedException">当該情報が破棄された場合</exception>
	private System.Text.StringBuilder BufferData => this.bufferData ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 項目一覧を取得します。
	/// </summary>
	/// <value>項目一覧</value>
	/// <exception cref="ObjectDisposedException">当該情報が破棄された場合</exception>
	private List<TParse> ExportList => this.exportList ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 要素番号を取得または設定します。
	/// </summary>
	/// <value>要素番号</value>
	private int RecordCode {
		get => this.recordCode;
		set => this.recordCode = value;
	}
	/// <summary>
	/// 状態種別を取得または設定します。
	/// </summary>
	/// <value>状態種別</value>
	private bool StringFlag {
		get => this.ignoreFlag;
		set => this.ignoreFlag = value;
	}
	#endregion プロパティー定義

	#region 開放メソッド定義
	/// <summary>
	/// 保持情報を開放します。
	/// </summary>
	~DsvDecodeData() {
		Dispose();
	}
	#endregion 開放メソッド定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	private void Dispose() {
		this.bufferData?.Clear();
		this.exportList?.Clear();
		this.parserHook = default;
		this.exportHook = default;
		this.escapeCode = default;
		this.markerCode = default;
		this.bufferData = default;
		this.exportList = default;
		this.recordCode = default;
	}
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		Dispose();
		GC.SuppressFinalize(this);
	}
	#endregion 破棄メソッド定義

	#region 内部メソッド定義:CreateData
	/// <summary>
	/// 要素情報を生成します。
	/// </summary>
	/// <param name="export">変換処理</param>
	/// <param name="record">項目一覧</param>
	/// <returns>要素情報</returns>
	private static TStore CreateData(DsvResultCode<TParse, TStore> export, List<TParse> record) {
		try {
			return export(record);
		} finally {
			record.Clear();
		}
	}
	#endregion 内部メソッド定義:CreateData

	#region 内部メソッド定義:DecodeItem
	/// <summary>
	/// 項目情報へ復号します。
	/// </summary>
	/// <param name="parser">解析処理</param>
	/// <param name="buffer">保留情報</param>
	/// <param name="column">情報番号</param>
	/// <param name="record">要素番号</param>
	/// <returns>項目情報</returns>
	/// <exception cref="StructException">項目情報の復号に失敗した場合</exception>
	private TParse DecodeItem(DsvParserCode<TParse> parser, System.Text.StringBuilder buffer, int column, int record) {
		try {
			var choose = buffer.ToString();
			if (StringFlag) {
				// 内容状態の場合：例外発行
				throw new StructException("DSV項目の文字列が終了していません。", choose, 0, choose.Length);
			} else {
				try {
					return parser(choose, column, record);
				} catch (StructException) {
					// 構造例外の場合：通過発行
					throw;
				} catch (Exception errors) {
					// 上記以外の場合：例外発行(構造例外へ変換)
					var value1 = "DSV項目の解析に失敗しました。";
					var value2 = choose;
					var value3 = 0;
					var value4 = choose.Length;
					throw new StructException(value1, value2, value3, value4, errors);
				}
			}
		} finally {
			buffer.Clear();
		}
	}
	#endregion 内部メソッド定義:DecodeItem

	#region 内部メソッド定義:DecodeData
	/// <summary>
	/// 要素情報へ復号します。
	/// </summary>
	/// <param name="export">変換処理</param>
	/// <param name="record">項目一覧</param>
	/// <param name="parser">解析処理</param>
	/// <param name="buffer">保留情報</param>
	/// <param name="source">読込文字</param>
	/// <param name="result">要素情報</param>
	/// <returns>要素情報の構築が完了した場合、<c>True</c>を返却</returns>
	/// <exception cref="StructException">項目情報の復号に失敗した場合</exception>
	private bool DecodeData(DsvResultCode<TParse, TStore> export, List<TParse> record, DsvParserCode<TParse> parser, System.Text.StringBuilder buffer, char source, [MaybeNullWhen(false)]out TStore result) {
		if (source == EscapeCode) {
			// 制御文字の場合：要素追加＋状態反転
			StringFlag = !StringFlag;
			buffer.Append(source);
			result = default;
			return false;
		} else if (StringFlag) {
			// 内容状態の場合：要素追加
			buffer.Append(source);
			result = default;
			return false;
		} else if (source == MarkerCode) {
			// 区切文字の場合：変換処理
			record.Add(DecodeItem(parser, buffer, record.Count, RecordCode));
			result = default;
			return false;
		} else if (1 <= buffer.Length && buffer[^1] == '\r') {
			// 改行情報の場合：要素判定
			if (source == '\n') {
				// 正規情報の場合：要素追加
				buffer.Length --;
				record.Add(DecodeItem(parser, buffer, record.Count, RecordCode));
				RecordCode ++;
				result = CreateData(export, record);
				return true;
			} else {
				// 異常状態の場合：例外発行
				var choose = buffer.ToString();
				buffer.Clear();
				throw new StructException("DSV項目に単一のキャリッジリターンが存在します。", choose, 0, choose.Length);
			}
		} else {
			buffer.Append(source);
			result = default;
			return false;
		}
	}
	/// <summary>
	/// 要素情報へ復号します。
	/// </summary>
	/// <param name="export">変換処理</param>
	/// <param name="record">項目一覧</param>
	/// <param name="parser">解析処理</param>
	/// <param name="buffer">保留情報</param>
	/// <param name="result">要素情報</param>
	/// <returns>要素情報の構築が完了した場合、<c>True</c>を返却</returns>
	/// <exception cref="StructException">項目情報の復号に失敗した場合</exception>
	private bool DecodeData(DsvResultCode<TParse, TStore> export, List<TParse> record, DsvParserCode<TParse> parser, System.Text.StringBuilder buffer, [MaybeNullWhen(false)]out TStore result) {
		if (record.Count <= 0 && buffer.Length <= 0) {
			// 保持情報なし
			result = default;
			return false;
		} else {
			// 保持情報あり
			record.Add(DecodeItem(parser, buffer, record.Count, RecordCode));
			result = CreateData(export, record);
			return true;
		}
	}
	#endregion 内部メソッド定義:DecodeData

	#region 公開メソッド定義:ImportData/FinishData
	/// <summary>
	/// 読込情報を取込みます。
	/// </summary>
	/// <param name="source">読込文字</param>
	/// <param name="result">要素情報</param>
	/// <returns>要素情報の構築が完了した場合、<c>True</c>を返却</returns>
	/// <exception cref="StructException">項目情報の復号に失敗した場合</exception>
	public bool ImportData(char source, [MaybeNullWhen(false)]out TStore result) =>
		DecodeData(ExportHook, ExportList, ParserHook, BufferData, source, out result);
	/// <summary>
	/// 終了処理を実行します。
	/// </summary>
	/// <param name="result">要素情報</param>
	/// <returns>要素情報の構築が完了した場合、<c>True</c>を返却</returns>
	/// <exception cref="StructException">項目情報の復号に失敗した場合</exception>
	public bool FinishData([MaybeNullWhen(false)]out TStore result) =>
		DecodeData(ExportHook, ExportList, ParserHook, BufferData, out result);
	#endregion 公開メソッド定義:ImportData/FinishData
}
