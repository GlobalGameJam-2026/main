# 인게임 UI 설정 가이드

## 🎮 개요

게임 화면 우측 상단에 3개의 버튼(나가기, 도움말, 재시작)을 배치하고 기능을 연결하는 방법입니다.

**구현된 기능:**
1. **X (나가기)**: 스테이지 선택 화면으로 돌아갑니다.
2. **? (도움말)**: 조작법 설명 팝업을 띄웁니다. (아무 키나 누르면 닫힘)
3. **↺ (재시작)**: 현재 스테이지를 처음부터 다시 시작합니다.
4. **단축키 지원**: `R`(재시작), `Backspace`(재시작), `X`(나가기), `?`/`F1`(도움말)

---

## 📋 설정 단계

### 1단계: UI Canvas 설정

모든 스테이지(Stage1, Stage2...)에 다음 작업을 수행해야 합니다.
(하나를 만들고 Prefab으로 저장해서 다른 씬에 복사하면 편합니다!)

1. **Canvas 생성**:
   - Hierarchy 우클릭 → UI → Canvas
   - 이름: `InGameUI`
   - **Canvas Scaler** 설정: `Scale With Screen Size`, Reference Resolution `1920 x 1080`

2. **InGameUIManager 추가**:
   - Canvas(또는 빈 오브젝트)에 **`InGameUIManager` 스크립트 추가**

---

### 2단계: 버튼 배치 (우측 상단)

1. **상단 패널 생성** (선택사항, 정렬용):
   - Canvas 우클릭 → Create Empty
   - 이름: `TopRightButtons`
   - Anchor Presets(Shift+Alt+클릭): **top-right** (우측 상단)
   - 위치: `Pos X: -20`, `Pos Y: -20` (여백)

2. **버튼 3개 생성**:
   - `TopRightButtons` 우클릭 → UI → Button (TextMeshPro)
   - 3개 생성 후 이름 변경: `Btn_Exit`, `Btn_Help`, `Btn_Restart`
   - **Horizontal Layout Group**을 부모(`TopRightButtons`)에 추가하면 자동 정렬됨 (선택사항)

3. **이미지 적용**:
   - 아까 Slice한 버튼 이미지 3개를 각각의 Button -> Image -> **Source Image**에 할당
   - `Set Native Size` 버튼 눌러서 비율 맞추기

---

### 3단계: 도움말 팝업(Panel) 만들기

1. **Panel 생성**:
   - Canvas 우클릭 → UI → Panel
   - 이름: `HelpPanel`
   - 색상: 검은색 (Alpha 200 정도)

2. **내용 추가**:
   - `HelpPanel` 안에 Text(TMP) 추가하여 설명글 작성
   - 또는 준비된 **도움말 이미지**가 있다면 Image 컴포넌트로 추가

3. **초기 상태**:
   - `HelpPanel`을 켜두기 (스크립트가 시작할 때 자동으로 끕니다)
   - 그래도 작업 편의상 끄고 싶다면 Inspector 상단 체크박스 해제

---

### 4단계: 기능 연결 (가장 중요!)

1. **InGameUIManager 컴포넌트 설정**:
   - Inspector의 **Help Popup Panel** 슬롯에 방금 만든 `HelpPanel` 드래그 할당

2. **버튼 이벤트 연결 (`On Click()`)**:

   **A. 나가기 버튼 (`Btn_Exit`)**
   - On Click (+) 추가
   - Object: `Canvas` (InGameUIManager가 있는 오브젝트)
   - Function: `InGameUIManager` -> **`ExitToStageSelect()`**

   **B. 도움말 버튼 (`Btn_Help`)**
   - On Click (+) 추가
   - Object: `Canvas`
   - Function: `InGameUIManager` -> **`ToggleHelpPopup()`**

   **C. 재시작 버튼 (`Btn_Restart`)**
   - On Click (+) 추가
   - Object: `Canvas`
   - Function: `InGameUIManager` -> **`RestartStage()`**

---

## 💡 팁
- **단축키**: 스크립트에 `R`, `Backspace` (재시작), `X` (나가기), `?` (도움말) 단축키가 이미 구현되어 있습니다. 도움말 이미지 내용과 일치합니다.
- **스테이지 선택 씬 이름**: `InGameUIManager`에서 `Stage Select Scene Name`이 "StageSelect"인지 확인하세요. (다르다면 수정 필요)

완성입니다! 이제 게임 플레이 중에도 UI로 편하게 제어할 수 있습니다. 🎮
