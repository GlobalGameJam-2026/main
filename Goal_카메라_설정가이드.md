# Goal & 카메라 설정 가이드

## 🎯 Goal 설정하기

### 1단계: Goal 프리팹에 스크립트 추가

1. **Project 창에서 Goal 프리팹 열기**
   - `Assets/Prefabs/Goal.prefab` 더블클릭

2. **Goal 컴포넌트 추가**
   - Add Component → `Goal`

3. **설정 (선택사항)**
   - Completion Sound: 사운드 파일 드래그 (나중에)
   - Next Scene Name: 다음 씬 이름 (비워두면 재시작)
   - Delay Before Next Level: 2초

4. **저장**
   - Ctrl+S 또는 상단 Save 버튼

---

## 📹 카메라 따라가기 설정

### 2단계: Main Camera에 CameraFollow 추가

1. **Hierarchy에서 Main Camera 선택**

2. **CameraFollow 컴포넌트 추가**
   - Add Component → `CameraFollow`

3. **설정**
   - **Auto Find Player**: ✓ (체크) - 자동으로 Player 찾음
   - **Offset**: (0, 2, -10) - 카메라 위치 조절
   - **Smooth Speed**: 5 - 부드러운 정도
   - **Use Dead Zone**: ✓ - 작은 움직임 무시
   - **Dead Zone Width**: 2
   - **Dead Zone Height**: 1
   - **Use Bounds**: ✗ - 일단 체크 해제 (맵이 크니까)

---

## 🎮 레벨 데이터 재생성

### 3단계: 더 크고 어려운 레벨로 업데이트

1. **Hierarchy에서 LevelCreator 선택**

2. **Inspector에서 우클릭**
   - **Create Stage 1 Data** 클릭
   - **Create Stage 2 Data** 클릭
   - "Do you want to replace?" → **Yes** 클릭

3. **확인**
   - Console에 "Larger & Harder" 메시지 확인

---

## 🎮 테스트하기

1. **Play 버튼** 클릭

2. **확인 사항**
   - ✓ 카메라가 플레이어를 따라감
   - ✓ 맵이 훨씬 넓어짐 (Stage 1: -12 ~ 18, Stage 2: -15 ~ 20)
   - ✓ 가면 전환 안 하면 진행 불가능
   - ✓ Goal에 도착하면 Console에 "🎉 Goal Reached!" 메시지
   - ✓ **R 키**로 레벨 재시작 가능

---

## ⚙️ 카메라 조절

### 더 멀리 보고 싶다면
- **Offset Y**: 2 → 3 또는 4로 증가

### 더 가까이 보고 싶다면
- **Offset Y**: 2 → 1 또는 0으로 감소

### 카메라가 너무 빠르게 움직인다면
- **Smooth Speed**: 5 → 3으로 감소

### 카메라가 너무 느리다면
- **Smooth Speed**: 5 → 8로 증가

---

## 📊 새로운 레벨 구성

### Stage 1 (더 크고 어려움)
- **길이**: -12 ~ 18 (총 30 유닛)
- **높이**: -4 ~ 4 (총 8 유닛)
- **플랫폼**: 13개
- **난이도**: 중간 - 가면 전환 필수

### Stage 2 (매우 어려움)
- **길이**: -15 ~ 20 (총 35 유닛)
- **높이**: -5 ~ 5 (총 10 유닛)
- **플랫폼**: 18개
- **난이도**: 어려움 - 정밀한 점프와 빠른 가면 전환 필요

---

## 💡 추가 팁

### Goal 도착 시 효과 추가하려면
1. Goal 프리팹 선택
2. Particle System 추가 (선택사항)
3. Goal 컴포넌트의 Completion Particles에 할당

### 맵 경계 설정하려면
1. Main Camera 선택
2. CameraFollow 컴포넌트에서:
   - Use Bounds: ✓
   - Min Bounds: (-15, -6)
   - Max Bounds: (22, 6)

---

## 🎯 다음 단계

이제 기본 게임이 완성되었습니다!

**추가 가능한 기능:**
- 감정 오브 수집 시스템
- 스테이지 선택 메뉴
- 사운드 효과
- 배경 음악
- 더 많은 메커니즘 (무너지는 플랫폼, 움직이는 플랫폼 등)

원하시는 기능 알려주세요!
